using AutoMapper;
using EventFoodOrders.AutoMapper;
using EventFoodOrders.Dto.EventDTOs;
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

        public ParticipantForResponseDto AddParticipantToEvent(string eventId, ParticipantForCreationDto newParticipant)
        {
            Guid id = Guid.Parse(eventId);

            Event desiredEvent = _eventRepository.GetSingleEventWithCondition(e => e.EventId == id);

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

        public ParticipantForResponseDto GetParticipant(string userId, string eventId)
        {
            Guid participantId = Guid.Parse(userId);

            Event participantsEvent = _eventRepository.GetEventForUser(userId, eventId);
            Participant? participant = participantsEvent.Participants
                .Where(p => p.participant_id == participantId)
                .FirstOrDefault();

            if (participant is null)
            {
                throw new ArgumentException($"No participant with id {userId} exists for event with id {eventId}.");
            }

            return _mapper.Map<ParticipantForResponseDto>(participant);
        }

        public IEnumerable<ParticipantForResponseDto> GetAllParticipantsForEvent(string userId, string eventId)
        {
            Event participantsEvent = _eventRepository.GetEventForUser(userId, eventId);
            IEnumerable<Participant> participants = [.. participantsEvent.Participants];

            return _mapper.Map<IEnumerable<ParticipantForResponseDto>>(participants);
        }
    }
}
