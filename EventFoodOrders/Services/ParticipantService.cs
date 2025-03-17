﻿using AutoMapper;
using EventFoodOrders.AutoMapper;
using EventFoodOrders.Dto.ParticipantDTOs;
using EventFoodOrders.Models;
using EventFoodOrders.Repositories;

namespace EventFoodOrders.Services
{
    public class ParticipantService(ParticipantRepository repository, EventRepository eventRepository, CustomAutoMapper mapper)
    {
        private readonly ParticipantRepository _participantRepository = repository;
        private readonly EventRepository _eventRepository = eventRepository;
        private readonly IMapper _mapper = mapper.Mapper;

        internal ParticipantForResponseDto AddParticipantToEvent(string eventId, ParticipantForCreationDto newParticipant)
        {
            Guid id = Guid.Parse(eventId);

            Event desiredEvent = _eventRepository.GetSingle(e => e.EventId == id);

            Participant participant = _mapper.Map<Participant>(newParticipant);
            participant.EventId = desiredEvent.EventId;

            _participantRepository.AddParticipant(participant);

            return _mapper.Map<ParticipantForResponseDto>(participant);
        }
    }
}
