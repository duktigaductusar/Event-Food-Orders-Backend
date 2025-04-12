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

public class EventService(IParticipantService participantService, IUoW uoW, ICustomAutoMapper mapper, IUserService userService, IMailerService mailerService) : IEventService
{
    private readonly IEventRepository _eventRepository = uoW.EventRepository;
    private readonly IParticipantRepository _participantRepository = uoW.ParticipantRepository;
    private readonly IMapper _mapper = mapper.Mapper;
    private readonly IUserService _userService = userService;

    public async Task<EventForResponseDto> CreateEvent(Guid ownerId, EventForCreationDto eventForCreation)
    {
        var newEvent = _eventRepository.AddEvent(
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
        participantService.AddParticipantsToNewEvent(newEvent, participantsToAdd);
        if (eventForCreation.UserIds?.Length != null && eventForCreation.UserIds?.Length > 0)
        {
            await mailerService.SendInvitationMail(eventForCreation, owner.UserId, newEvent.Id);
        }
        await mailerService.SendCreatorConfirmationMail(eventForCreation, owner.UserId, newEvent.Id);

        return _mapper.MapToEventForResponseDto(newEvent, owner);
    }

    public EventForResponseDto UpdateEvent(Guid eventId, Guid ownerId, EventForUpdateDto updatedEventDto)
    {
        var eventToUpdate = _eventRepository.GetEventForUser(ownerId, eventId);
        var updatedEvent = _mapper.MapToEventFromUpdateDto(updatedEventDto, eventId, ownerId);

        var participantsToDelete = eventToUpdate.Participants
            .Where(p => !(updatedEventDto.UserIds ?? []).Contains(p.UserId) && p.UserId != ownerId)
            .ToList();

        foreach (Participant participant in participantsToDelete)
        {
            participantService.DeleteParticipant(participant.Id);
        }

        var existingParticipantIds = eventToUpdate.Participants
            .Select(p => p.UserId)
            .ToHashSet();

        var participantsToAdd = new List<Participant>();

        foreach (Guid id in updatedEventDto.UserIds ?? [])
        {
            if (existingParticipantIds.Contains(id) == false)
            {
                participantsToAdd.Add(new Participant
                {
                    UserId = id,
                    EventId = eventId
                });
            }
        }

        participantService.AddParticipantsToNewEvent(updatedEvent, participantsToAdd);
        updatedEvent = _eventRepository.UpdateEvent(eventId, updatedEvent);
        return _mapper.Map<EventForResponseDto>(updatedEvent);
    }

    public bool DeleteEvent(Guid userId, Guid eventId)
    {
        _eventRepository.DeleteEvent(userId, eventId);

        return true;
    }

    public EventForResponseWithDetailsDto GetEventForUser(Guid userId, Guid eventId)
    {
        var returnEvent = _eventRepository.GetEventForUser(userId, eventId);
        var eventParticipant = _participantRepository.GetParticipantWithEventAndUserId(eventId, userId);

        return eventParticipant == null
            ? throw new EventNotFoundException(eventId)
            : _mapper.MapToEventForResponseWithDetailsDto(returnEvent, eventParticipant);
    }

    public IEnumerable<EventForResponseDto> GetAllEventsForUser(Guid userId)
    {
        IEnumerable<Event> returnEvents = _eventRepository.GetAllEventsForUser(userId);
        List<EventForResponseDto> events = [];

        // string name = _userService.GetUserWithId(userId);

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
