using AutoMapper;
using EventFoodOrders.AutoMapper;
using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Dto.ParticipantDTOs;
using EventFoodOrders.Models;
using EventFoodOrders.Repositories;
using Microsoft.Extensions.Logging;

namespace EventFoodOrders.Services
{
    public class ParticipantService(ParticipantRepository repository, EventRepository eventRepository, CustomAutoMapper mapper)
    {
        private readonly ParticipantRepository _participantRepository = repository;
        private readonly EventRepository _eventRepository = eventRepository;
        private readonly IMapper _mapper = mapper.Mapper;

        public ParticipantForResponseDto AddParticipantToEvent(string eventId, ParticipantForCreationDto newParticipant)
        {
            Guid id = Guid.Parse(eventId);

            Event desiredEvent = _eventRepository.GetSingle(e => e.EventId == id);

            Participant participant = _mapper.Map<Participant>(newParticipant);
            participant.EventId = desiredEvent.EventId;

            _participantRepository.AddParticipant(participant);

            return _mapper.Map<ParticipantForResponseDto>(participant);
        }

        public ParticipantForResponseDto UpdateParticipant(string participantId, ParticipantForUpdateDto updatedParticipantDto)
        {
            Participant updatedParticipant = _mapper.Map<Participant>(updatedParticipantDto);

            updatedParticipant = _participantRepository.UpdateParticipant(participantId, updatedParticipant);

            return _mapper.Map<ParticipantForResponseDto>(updatedParticipant);
        }

        public bool DeleteParticipant(string participantId)
        {
            try
            {
                _participantRepository.DeleteParticipant(participantId);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
