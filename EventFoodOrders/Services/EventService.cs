﻿using AutoMapper;
using EventFoodOrders.AutoMapper;
using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Models;
using EventFoodOrders.Repositories;

namespace EventFoodOrders.Services;

public class EventService(EventRepository eventRepository, ParticipantRepository participantRepository, CustomAutoMapper mapper)
{
    private readonly EventRepository _eventRepository = eventRepository;
    private readonly ParticipantRepository _participantRepository = participantRepository;
    private readonly IMapper _mapper = mapper.Mapper;

    public EventForResponseWithDetailsDto CreateEvent(EventForCreationDto eventForCreation)
    {
        Event newEvent = _mapper.Map<Event>(eventForCreation);
        newEvent.Id = Guid.NewGuid();

        newEvent = _eventRepository.AddEvent(newEvent);

        return _mapper.Map<EventForResponseWithDetailsDto>(newEvent);
    }

    public EventForResponseWithDetailsDto UpdateEvent(string eventId, EventForUpdateDto updatedEventDto)
    {
        Event updatedEvent = _mapper.Map<Event>(updatedEventDto);

        updatedEvent = _eventRepository.UpdateEvent(eventId, updatedEvent);

        return _mapper.Map<EventForResponseWithDetailsDto>(updatedEvent);
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

        return _mapper.MapToEventForResponseDto(returnEvent, eventParticipant);
    }

    public IEnumerable<EventForResponseWithDetailsDto> GetAllEventsForUser(string userId)
    {
        IEnumerable<Event> returnEvents = _eventRepository.GetAllEventsForUser(userId);

        return _mapper.Map<IEnumerable<EventForResponseWithDetailsDto>>(returnEvents);
    }
}
