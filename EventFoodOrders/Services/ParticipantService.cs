using AutoMapper;
using EventFoodOrders.AutoMapper;
using EventFoodOrders.Dto.ParticipantDTOs;
using EventFoodOrders.Entities;
using EventFoodOrders.Exceptions;
using EventFoodOrders.Repositories.Interfaces;
using EventFoodOrders.Services.Interfaces;

namespace EventFoodOrders.Services;

public class ParticipantService(IUoW uoW, ICustomAutoMapper mapper) : IParticipantService
{
    private readonly IParticipantRepository _participantRepository = uoW.ParticipantRepository;
    private readonly IEventRepository _eventRepository = uoW.EventRepository;
    private readonly IMapper _mapper = mapper.Mapper;

    public ParticipantForResponseDto AddParticipantToEvent(Guid eventId, ParticipantForCreationDto newParticipant)
    {
        Event? desiredEvent = _eventRepository.GetSingleEventWithCondition(e => e.Id == eventId) ?? throw new EventNotFoundException();
        if (desiredEvent.Participants.Where(p => p.UserId == newParticipant.UserId).Any())
        {
            // This could be more specific but would reveal that a user is invited to an event.
            throw new EventNotFoundException();
        }

        Participant? existingParticipant = _participantRepository.GetParticipantWithUserId(newParticipant.UserId);
        Participant participant = _mapper.MapToParticipantFromCreationDto(eventId, newParticipant);

        if (existingParticipant is not null)
        {
            participant.Allergies = existingParticipant.Allergies;
            participant.Preferences = existingParticipant.Preferences;
        }

        _participantRepository.AddParticipant(participant);
        return _mapper.Map<ParticipantForResponseDto>(participant);
    }

    public ParticipantForResponseDto UpdateParticipant(Guid participantId, ParticipantForUpdateDto updatedParticipantDto)
    {
        Participant participant = _participantRepository.GetParticipantWithParticipantId(participantId) ?? throw new ParticipantNotFoundException();
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
            throw new ParticipantNotFoundInEventException(userId, eventId);
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
