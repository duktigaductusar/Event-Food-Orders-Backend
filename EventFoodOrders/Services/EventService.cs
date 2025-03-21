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

        Participant owner = _participantService.CreateParticipant(userId, newEvent);
        owner = _participantRepository.AddParticipant(owner);

        return _mapper.MapToEventForResponseDto(newEvent, owner);
    }

    public EventForResponseDto UpdateEvent(string eventId, EventForUpdateDto updatedEventDto)
    {
        Event updatedEvent = _mapper.Map<Event>(updatedEventDto);

        updatedEvent = _eventRepository.UpdateEvent(eventId, updatedEvent);

        return _mapper.Map<EventForResponseDto>(updatedEvent);
    }

    public bool DeleteEvent(string eventId)
    {
        _eventRepository.DeleteEvent(eventId);

        return true;
    }

    public EventForResponseWithDetailsDto GetEventForUser(string userId, string eventId)
    {
        Event returnEvent = _eventRepository.GetEventForUser(userId, eventId);
        Participant eventParticipant = _participantRepository.GetParticipant(userId);

        return _mapper.MapToEventForResponseWithDetailsDto(returnEvent, eventParticipant);
    }

    public IEnumerable<EventForResponseDto> GetAllEventsForUser(string userId)
    {
        IEnumerable<Event> returnEvents = _eventRepository.GetAllEventsForUser(userId);
        IEnumerable<EventForResponseDto> events = [];

        foreach(Event e in returnEvents)
        {
            Participant? participant = e.Participants
                .Where(p => p.UserId == Guid.Parse(userId))
                .FirstOrDefault();

            if (participant is not null)
            {
                events.Append(_mapper.MapToEventForResponseDto(e, participant));
            }/*
            else
            {
                ParticipantBuilder builder = new();
                builder.SetEvent(e);
                builder.SetAllergies(["Shrimp"]);
                participant = builder.BuildParticipant();
                events = events.Append(_mapper.MapToEventForResponseDto(e, participant));
            }*/
        }

        return events;
    }
}
