using AutoMapper;
using EventFoodOrders.AutoMapper;
using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Models;
using EventFoodOrders.Repositories;

namespace EventFoodOrders.Services;

public class EventService(EventRepository repository, CustomAutoMapper mapper)
{
    private readonly EventRepository _repository = repository;
    private readonly IMapper _mapper = mapper.Mapper;

    public EventForResponseDto CreateEvent(EventForCreationDto eventForCreation)
    {
        Event newEvent = _mapper.Map<Event>(eventForCreation);
        newEvent.EventId = Guid.NewGuid();

        newEvent = _repository.AddEvent(newEvent);

        return _mapper.Map<EventForResponseDto>(newEvent);
    }

    public EventForResponseDto UpdateEvent(string eventId, EventForUpdateDto updatedEventDto)
    {
        Event updatedEvent = _mapper.Map<Event>(updatedEventDto);

        updatedEvent = _repository.UpdateEvent(eventId, updatedEvent);

        return _mapper.Map<EventForResponseDto>(updatedEvent);
    }

    public bool DeleteEvent(string eventId)
    {
        _repository.DeleteEvent(eventId);

        return true;
    }

    public EventForResponseDto GetEventForUser(string userId, string eventId)
    {
        Event returnEvent = _repository.GetEventForUser(userId, eventId);

        return _mapper.Map<EventForResponseDto>(returnEvent);
    }

    public IEnumerable<EventForResponseDto> GetAllEventsForUser(string userId)
    {
        IEnumerable<Event> returnEvents = _repository.GetAllEventsForUser(userId);

        return _mapper.Map<IEnumerable<EventForResponseDto>>(returnEvents);
    }
}
