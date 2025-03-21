using AutoMapper;
using EventFoodOrders.AutoMapper;
using EventFoodOrders.Dto.ParticipantDTOs;
using EventFoodOrders.Entities;
using EventFoodOrders.Repositories;

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
        _participantRepository.AddParticipant(participant);

        return _mapper.Map<ParticipantForResponseDto>(participant);
    }

    public ParticipantForResponseDto UpdateParticipant(string participantId, ParticipantForUpdateDto updatedParticipantDto)
    {
        Participant participant = _participantRepository.GetParticipant(participantId);
        participant = _mapper.MapToParticipantFromUpdateDto(participant, updatedParticipantDto);
        participant = _participantRepository.UpdateParticipant(participantId, participant);

        return _mapper.Map<ParticipantForResponseDto>(participant);
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

    public Participant CreateParticipant(Guid userId, Event eventToParticipateIn)
    {
        return new Participant(userId, eventToParticipateIn);
    }
}
