using AutoMapper;
using EventFoodOrders.AutoMapper;
using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Dto.ParticipantDTOs;
using EventFoodOrders.Dto.UserDTOs;
using EventFoodOrders.Exceptions;
using EventFoodOrders.Repositories.Interfaces;
using EventFoodOrders.Services.Interfaces;
using EventFoodOrders.Utilities;
using Microsoft.Extensions.Logging;
using Event = EventFoodOrders.Entities.Event;
using Participant = EventFoodOrders.Entities.Participant;

namespace EventFoodOrders.Services;

public class EventService(
    IParticipantService participantService,
    IUserService userService,
    IUoW uoW,
    ICustomAutoMapper mapper,
    IMailManager mailManager
) : IEventService
{
    private readonly IEventRepository _eventRepository = uoW.EventRepository;
    private readonly IParticipantRepository _participantRepository = uoW.ParticipantRepository;
    private readonly IMapper _mapper = mapper.Mapper;
    private readonly IUserService _userService = userService;
    private readonly IParticipantService _participantService = participantService;

    public async Task<EventForResponseDto> CreateEvent(Guid ownerId, EventForCreationDto eventForCreation)
    {
        var newEvent = await _eventRepository.AddEvent(
            _mapper.MapToNewEvent(ownerId, eventForCreation));

        var participantsToAdd = new List<Participant>();

        var owner = new Participant
        {
            UserId = ownerId,
            EventId = newEvent.Id,
            WantsMeal = true,
            ResponseType = ReType.AttendingOffice
        };
        participantsToAdd.Add(owner);

        foreach (Guid userId in eventForCreation.UserIds ?? [])
        {
            if (await _userService.GetUserWithId(userId) is null)
            {
                List<Guid> usersInGroup = await _userService.GetUsersFromGroup(userId);
                usersInGroup.Remove(ownerId);
                foreach (Guid userIdFromGroup in usersInGroup)
                {
                    participantsToAdd.Add(new Participant
                    {
                        UserId = userIdFromGroup,
                        EventId = newEvent.Id,
                    });
                }
            }
            else
            {
                participantsToAdd.Add(new Participant
                {
                    UserId = userId,
                    EventId = newEvent.Id,
                });
            }
        }
        
        // TODO! Use Event instaed of DTO participant
        var participants = await _participantService.AddParticipantsToEvent(newEvent, participantsToAdd);

        var focusedEvent = eventForCreation with
        {
            UserIds = participantsToAdd
                .Select(p => p.UserId)
                .ToArray()
        };

        // TODO! Mail Should be done after all transition, use in separate method
        // (e.g) in a MailManagerService instance for clean code.
        //if (eventForCreation.UserIds?.Length != null && eventForCreation.UserIds?.Length > 0)
        //{
        //    await mailerService.SendInvitationMail(focusedEvent, owner.UserId, newEvent.Id);
        //}
        //await mailerService.SendCreatorConfirmationMail(eventForCreation, owner.UserId, newEvent.Id);
        await mailManager.HandleNewEventMails(
            focusedEvent, ownerId, newEvent.Id);
        // End mail

        return _mapper.MapToEventForResponseDto(newEvent, owner);
    }

    public async Task<EventForResponseDto> UpdateEvent(Guid eventId, Guid ownerId, EventForUpdateDto updatedEventDto)
    {
        var eventToUpdate = await _eventRepository.GetEventForUser(ownerId, eventId);
        var updatedEvent = _mapper.MapToEventFromUpdateDto(updatedEventDto, eventId, ownerId);

        var participantsToDelete = eventToUpdate.Participants
            .Where(p => !(updatedEventDto.UserIds ?? []).Contains(p.UserId) && p.UserId != ownerId)
            .ToList();

        // TODO! FIx Buggy solution. First users may get revoked email notifcation mail.
        //      Then the user may be in a group and in this method and the receive a new invitaion
        //      mail to the same event. This will be bad UX.
        // FIX! Handle after database transation in seaprate manager class/method.
        // TODO! Mail Should be done after all transition, use in separate method/class
        //      (e.g) in a MailManagerService instance for clean code. 
        //await mailerService.SendRevokeInvitationMail(
        //   eventToUpdate, participantsToDelete.Select(p => p.UserId));
        
        // End mail

        foreach (Participant participant in participantsToDelete)
        {
            await _participantService.DeleteParticipant(participant.Id);
        }

        var existingParticipantIds = eventToUpdate.Participants
            .Select(p => p.UserId)
            .ToHashSet();

        var participantsToAdd = new List<Participant>();

        foreach (Guid userId in updatedEventDto.UserIds ?? [])
        {
            if (await _userService.GetUserWithId(userId) is null)
            {
                List<Guid> usersInGroup = await _userService.GetUsersFromGroup(userId);
                usersInGroup.Remove(ownerId);
                foreach (Guid userIdFromGroup in usersInGroup)
                {
                    if (existingParticipantIds.Contains(userId) == false)
                    {
                        participantsToAdd.Add(new Participant
                        {
                            UserId = userIdFromGroup,
                            EventId = eventId,
                        });
                    }
                }
            }
            else if (existingParticipantIds.Contains(userId) == false)
            {
                participantsToAdd.Add(new Participant
                {
                    UserId = userId,
                    EventId = eventId
                });
            }
        }

        updatedEvent = await _eventRepository.UpdateEvent(eventId, updatedEvent);

        await _participantService.AddParticipantsToEvent(updatedEvent, participantsToAdd);

        // TODO! Mail Should be done after all transition, use in separate method/class
        // (e.g) in a MailManagerService instance for clean code.
        var focusedEvent = updatedEventDto with
        {
            UserIds = participantsToAdd
                .Select(p => p.UserId)
                .ToArray()
        };
        //if (updatedEventDto.UserIds?.Length != null && updatedEventDto.UserIds?.Length > 0)
        //{
        //    await mailerService.SendInvitationMail(focusedEvent, ownerId, eventId);
        //}
        //await mailerService.SendCreatorConfirmationMail(focusedEvent, ownerId, eventId);
        await mailManager.HandleUpdateEventMails(
            updatedEventDto,
            updatedEvent,
            participantsToAdd,
            participantsToDelete,
            ownerId,
            eventId);
        // End mail

        return _mapper.Map<EventForResponseDto>(updatedEvent);
    }

    public async Task<bool> DeleteEvent(Guid ownerId, Guid eventId)
    {
        var eventToDelete = await _eventRepository.GetEventByIdWithParticipants(eventId);

        if (eventToDelete != null) {
            await _eventRepository.DeleteEvent(ownerId, eventId);
            // TODO! Mail Should be done after all transition, use in separate method/class
            // (e.g) in a MailManagerService instance for clean code.

            //var eventParticipants = eventToDelete.Participants
            //        .Where(p => p.UserId != ownerId)
            //        .Select(p => p.UserId)
            //        .ToList();

            //await mailerService.SendEventCanceledMail(
            //    eventToDelete, eventParticipants);

            //await mailerService.SendDeleteConfirmationMail(eventToDelete, ownerId);
            await mailManager.HandleCancelEventMails(eventToDelete, ownerId);
            // End mail
        }

        return true;
    }

    public async Task<EventForResponseWithDetailsDto> GetEventForUser(Guid userId, Guid eventId)
    {
        var returnEvent = await _eventRepository.GetEventForUser(userId, eventId);
        var eventParticipant = await _participantRepository.GetParticipantWithEventAndUserId(eventId, userId);

        return eventParticipant == null
            ? throw new EventNotFoundException(eventId)
            : _mapper.MapToEventForResponseWithDetailsDto(returnEvent, eventParticipant);
    }

    public async Task<IEnumerable<EventForResponseDto>> GetAllEventsForUser(Guid userId)
    {
        IEnumerable<Event> returnEvents = await _eventRepository.GetAllEventsForUser(userId);
        List<EventForResponseDto> events = [];

        foreach (Event e in returnEvents)
        {
            Participant? participant = e.Participants
                .Where(p => p.UserId == userId)
                .FirstOrDefault();

            if (participant is not null)
            {
                events.Add(_mapper.MapToEventForResponseDto(e, participant));
            }
        }
        events.Sort((i, p) => i.Title.CompareTo(p.Title));
        events.Sort((i, p) => i.Date.CompareTo(p.Date));
        return events;
    }

    public EventForResponseWithUsersDto GetEventWithUsers(EventForResponseWithDetailsDto eventDto, IEnumerable<ParticipantForResponseDto> participantDtos, IEnumerable<UserDto> users)
    {
        return _mapper.MapToEventForResponseWithUsersDto(eventDto, participantDtos, users);
    }
}
