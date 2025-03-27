using AutoMapper;
using EventFoodOrders.AutoMapper;
using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Entities;
using EventFoodOrders.Repositories.Interfaces;
using EventFoodOrders.Services.Interfaces;

namespace EventFoodOrders.Services;

public class EventService(IParticipantService participantService, IUoW uoW, ICustomAutoMapper mapper, IUserService userService) : IEventService
{
    private readonly IEventRepository _eventRepository = uoW.EventRepository;
    private readonly IParticipantRepository _participantRepository = uoW.ParticipantRepository;
    private readonly IParticipantService _participantService = participantService;
    private readonly IMapper _mapper = mapper.Mapper;
    private readonly IUserService _userService = userService;

    public EventForResponseDto CreateEvent(Guid userId, EventForCreationDto eventForCreation)
    {
        Event newEvent = _mapper.MapToNewEvent(userId, eventForCreation);
        newEvent = _eventRepository.AddEvent(newEvent);

        Participant owner = _participantService.CreateParticipant(userId, newEvent.Id);
        owner = _participantRepository.AddParticipant(owner);

        return _mapper.MapToEventForResponseDto(newEvent, owner);
    }

    public EventForResponseDto UpdateEvent(Guid eventId, EventForUpdateDto updatedEventDto)
    {
        Event updatedEvent = _mapper.Map<Event>(updatedEventDto);

        updatedEvent = _eventRepository.UpdateEvent(eventId, updatedEvent);

        return _mapper.Map<EventForResponseDto>(updatedEvent);
    }

    public bool DeleteEvent(Guid eventId)
    {
        _eventRepository.DeleteEvent(eventId);

        return true;
    }

    public EventForResponseWithDetailsDto GetEventForUser(Guid userId, Guid eventId)
    {
        Event returnEvent = _eventRepository.GetEventForUser(userId, eventId);
        Participant eventParticipant = _participantRepository.GetParticipantWithUserId(userId);

        return _mapper.MapToEventForResponseWithDetailsDto(returnEvent, eventParticipant);
    }

    public IEnumerable<EventForResponseDto> GetAllEventsForUser(Guid userId)
    {
        IEnumerable<Event> returnEvents = _eventRepository.GetAllEventsForUser(userId);
        List<EventForResponseDto> events = [];

        string name = _userService.GetNameWithId(Guid.Parse("a84c12d5-9075-42d2-b467-6b345b7d8c9f"));

        foreach (Event e in returnEvents)
        {
            Participant? participant = e.Participants
                //.Where(p => p.UserId == userId)
                .FirstOrDefault();

            if (participant is not null)
            {
                events.Add(_mapper.MapToEventForResponseDto(e, participant));
            }
        }

        return events;
    }
}
