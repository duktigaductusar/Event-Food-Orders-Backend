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

    public ParticipantForResponseDto AddParticipantToEvent(Guid eventId, ParticipantForCreationDto newParticipant)
    {
       Event desiredEvent = _eventRepository.GetSingleEventWithCondition(e => e.Id == eventId);

        //Participant participant = _mapper.MapToParticipantFromCreationDto(eventId, newParticipant);
        Participant participant = _mapper.MapToParticipantFromCreationDto(eventId, newParticipant);
        _participantRepository.AddParticipant(participant);

        return _mapper.Map<ParticipantForResponseDto>(participant);
    }

    public ParticipantForResponseDto UpdateParticipant(Guid participantId, ParticipantForUpdateDto updatedParticipantDto)
    {
        Participant participant = _participantRepository.GetParticipant(participantId);
        participant = _mapper.MapToParticipantFromUpdateDto(participant, updatedParticipantDto);
        _participantRepository.UpdateParticipant(participantId, participant);

        return _mapper.Map<ParticipantForResponseDto>(participant);
    }

    public bool DeleteParticipant(Guid participantId)
    {
        _participantRepository.DeleteParticipant(participantId);

        return true;
    }

    public ParticipantForResponseDto GetParticipant(Guid userId, Guid eventId)
    {
        Event participantsEvent = _eventRepository.GetEventForUser(userId, eventId);
        Participant? participant = participantsEvent.Participants
            .Where(p => p.Id == userId)
            .FirstOrDefault();

        if (participant is null)
        {
            throw new ArgumentException($"No participant with id {userId} exists for event with id {eventId}.");
        }

        return _mapper.Map<ParticipantForResponseDto>(participant);
    }

    public IEnumerable<ParticipantForResponseDto> GetAllParticipantsForEvent(Guid userId, Guid eventId)
    {
        Event participantsEvent = _eventRepository.GetEventForUser(userId, eventId);
        IEnumerable<Participant> participants = [.. participantsEvent.Participants];

        return _mapper.Map<IEnumerable<ParticipantForResponseDto>>(participants); ;
    }

    public IEnumerable<ParticipantForResponseDto> GetAllParticipantsForUser(Guid userId)
    {
        IEnumerable<Participant> participants = _participantRepository.GetAllParticipantsForUser(userId);

        return _mapper.Map<IEnumerable<ParticipantForResponseDto>>(participants);
    }

    public Participant CreateParticipant(Guid userId, Guid eventId)
    {
        return new Participant(userId, eventId);
    }
}
