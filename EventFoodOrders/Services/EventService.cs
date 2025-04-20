using AutoMapper;
using EventFoodOrders.AutoMapper;
using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Dto.ParticipantDTOs;
using EventFoodOrders.Dto.UserDTOs;
using EventFoodOrders.Exceptions;
using EventFoodOrders.Repositories.Interfaces;
using EventFoodOrders.Services.Interfaces;
using EventFoodOrders.Utilities;
using Event = EventFoodOrders.Entities.Event;
using Participant = EventFoodOrders.Entities.Participant;

namespace EventFoodOrders.Services;

public class EventService(
    IUoW uoW,
    IServiceManager sm,
    ICustomAutoMapper mapper
) : IEventService
{
    private readonly IMapper _mapper = mapper.Mapper;

    public async Task<EventForResponseDto> CreateEvent(Guid ownerId, EventForCreationDto eventForCreation)
    {
        var newEvent = await uoW.EventRepository.AddEvent(
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
            if (await sm.UserService.GetUserWithId(userId) is null)
            {
                List<Guid> usersInGroup = await sm.UserService.GetUsersFromGroup(userId);
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

        var participants = await sm.ParticipantService.AddParticipantsToEvent(
            newEvent, participantsToAdd);

        await sm.MailManager.HandleNewEventMails(
            newEvent, participantsToAdd.Select(p => p.UserId).ToHashSet());

        return _mapper.MapToEventForResponseDto(newEvent, owner);
    }

    public async Task<EventForResponseDto> UpdateEvent(Guid eventId, Guid ownerId, EventForUpdateDto updatedEventDto)
    {
        var eventToUpdate = await uoW.EventRepository.GetEventForUser(ownerId, eventId);
        // Keep original values for comparison
        var originalTitle = eventToUpdate.Title;
        var originalStartTime = eventToUpdate.Date;
        var originalEndTime = eventToUpdate.EndTime;
        var originalDescription = eventToUpdate.Description;

        var updatedEvent = _mapper.MapToEventFromUpdateDto(updatedEventDto, eventId, ownerId);

        var participantsToDelete = eventToUpdate.Participants
            .Where(p => !(updatedEventDto.UserIds ?? []).Contains(p.UserId) && p.UserId != ownerId)
            .ToList();

        foreach (Participant participant in participantsToDelete)
        {
            await sm.ParticipantService.DeleteParticipant(participant.Id);
        }

        var existingParticipantIds = eventToUpdate.Participants
            .Select(p => p.UserId)
            .ToHashSet();

        var participantsToAdd = new List<Participant>();

        foreach (Guid userId in updatedEventDto.UserIds ?? [])
        {
            if (await sm.UserService.GetUserWithId(userId) is null)
            {
                List<Guid> usersInGroup = await sm.UserService.GetUsersFromGroup(userId);
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

        updatedEvent = await uoW.EventRepository.UpdateEvent(eventId, updatedEvent);

        await sm.ParticipantService.AddParticipantsToEvent(updatedEvent, participantsToAdd);

        var eventDetailsChanged =
            originalTitle != updatedEvent.Title ||
            originalStartTime != updatedEvent.Date ||
            originalEndTime != updatedEvent.EndTime ||
            originalDescription != updatedEvent.Description;

        await sm.MailManager.HandleUpdateEventMails(
            updatedEvent,
            eventDetailsChanged ? updatedEvent.Participants : participantsToAdd,
            participantsToDelete);

        return _mapper.Map<EventForResponseDto>(updatedEvent);
    }

    public async Task<bool> DeleteEvent(Guid ownerId, Guid eventId)
    {
        var eventToDelete = await uoW.EventRepository.GetEventByIdWithParticipants(eventId);

        if (eventToDelete != null)
        {
            await uoW.EventRepository.DeleteEvent(ownerId, eventId);
            await sm.MailManager.HandleCancelEventMails(eventToDelete, ownerId);
        }

        return true;
    }

    public async Task<EventForResponseWithDetailsDto> GetEventForUser(Guid userId, Guid eventId)
    {
        var returnEvent = await uoW.EventRepository.GetEventForUser(userId, eventId);
        var eventParticipant = await uoW.ParticipantRepository.GetParticipantWithEventAndUserId(eventId, userId);

        return eventParticipant == null
            ? throw new EventNotFoundException(eventId)
            : _mapper.MapToEventForResponseWithDetailsDto(returnEvent, eventParticipant);
    }

    public async Task<IEnumerable<EventForResponseDto>> GetAllEventsForUser(Guid userId)
    {
        IEnumerable<Event> returnEvents = await uoW.EventRepository.GetAllEventsForUser(userId);
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
