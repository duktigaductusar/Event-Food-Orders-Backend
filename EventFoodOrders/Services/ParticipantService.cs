using AutoMapper;
using EventFoodOrders.AutoMapper;
using EventFoodOrders.Dto.ParticipantDTOs;
using EventFoodOrders.Models;
using EventFoodOrders.Repositories;
using EventFoodOrders.Utilities;

namespace EventFoodOrders.Services;

public class ParticipantService(ParticipantRepository repository, EventRepository eventRepository, CustomAutoMapper mapper)
{
    private readonly ParticipantRepository _participantRepository = repository;
    private readonly EventRepository _eventRepository = eventRepository;
    private readonly IMapper _mapper = mapper.Mapper;

    public ParticipantForResponseDto AddParticipantToEvent(string eventId, ParticipantForCreationDto newParticipant)
    {
        Guid id = Guid.Parse(eventId);

        Event desiredEvent = _eventRepository.GetSingleEventWithCondition(e => e.Id == id);

        Participant participant = _mapper.Map<Participant>(newParticipant);
        participant.EventId = desiredEvent.Id;
        participant.Id = Guid.NewGuid();
        participant.Response = ReType.Pending;

        _participantRepository.AddParticipant(participant);

        return _mapper.Map<ParticipantForResponseDto>(participant);
    }

    public ParticipantForResponseDto UpdateParticipant(string participantId, ParticipantForUpdateDto updatedParticipantDto)
    {
        Participant updatedParticipant = _participantRepository.GetParticipant(participantId);
        updatedParticipant = _mapper.Map(updatedParticipantDto, updatedParticipant);

        updatedParticipant = _participantRepository.UpdateParticipant(participantId, updatedParticipant);

        return _mapper.Map<ParticipantForResponseDto>(updatedParticipant);
    }

    public bool DeleteParticipant(string participantId)
    {
        _participantRepository.DeleteParticipant(participantId);

        return true;
    }

    public ParticipantForResponseDto GetParticipant(string userId, string eventId)
    {
        Guid participantId = Guid.Parse(userId);

        Event participantsEvent = _eventRepository.GetEventForUser(userId, eventId);
        Participant? participant = participantsEvent.Participants
            .Where(p => p.Id == participantId)
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

        return _mapper.Map<IEnumerable<ParticipantForResponseDto>>(participants); ;
    }

    public IEnumerable<ParticipantForResponseDto> GetAllParticipantsForUser(string userId)
    {
        IEnumerable<Participant> participants = _participantRepository.GetAllParticipantsForUser(userId);

        return _mapper.Map<IEnumerable<ParticipantForResponseDto>>(participants);
    }
}
