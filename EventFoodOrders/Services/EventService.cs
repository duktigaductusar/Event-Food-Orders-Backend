using AutoMapper;
using EventFoodOrders.AutoMapper;
using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Entities;
using EventFoodOrders.Repositories;

namespace EventFoodOrders.Services;

public class EventService(EventRepository eventRepository, ParticipantService participantService, ParticipantRepository participantRepository, CustomAutoMapper mapper)
{
    private readonly EventRepository _eventRepository = eventRepository;
    private readonly ParticipantService _participantService = participantService;
    private readonly ParticipantRepository _participantRepository = participantRepository;
    private readonly IMapper _mapper = mapper.Mapper;

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
        Participant eventParticipant = _participantRepository.GetParticipantWithParticipantId(userId);

        return _mapper.MapToEventForResponseWithDetailsDto(returnEvent, eventParticipant);
    }

    public IEnumerable<EventForResponseDto> GetAllEventsForUser(Guid userId)
    {
        IEnumerable<Event> returnEvents = _eventRepository.GetAllEventsForUser(userId);
        List<EventForResponseDto> events = [];

        foreach(Event e in returnEvents)
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
