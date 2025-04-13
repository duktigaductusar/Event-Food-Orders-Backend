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

    public async Task<IEnumerable<Participant>> AddParticipantsToEvent(Event newEvent, IEnumerable<Participant> participants)
    {
        var userIds = participants.Select(p => p.UserId).Distinct();
        var orderedAttendingOfficeParticipants = await _eventRepository.GetAttendingOfficeParticipantsDescendingByUpdate(userIds);

        foreach (var participant in participants)
        {
            var latestParticpantForUser = orderedAttendingOfficeParticipants
                .Where(p => p.UserId == participant.UserId)
                .FirstOrDefault();

            if (latestParticpantForUser is not null)
            {
                participant.Allergies = latestParticpantForUser.Allergies;
                participant.Preferences = latestParticpantForUser.Preferences;
            }
        }

        return await _participantRepository.AddParticipants(participants);
    }

    public async Task<ParticipantForResponseDto> UpdateParticipant(Guid participantId, ParticipantForUpdateDto dto)
    {
        Participant participant = await _participantRepository.GetParticipantWithParticipantId(participantId) ?? throw new ParticipantNotFoundException();
        participant = _mapper.MapToParticipantFromUpdateDto(participant, dto);
        await _participantRepository.UpdateParticipant(participantId, participant);

        return _mapper.Map<ParticipantForResponseDto>(participant);
    }

    public async Task<ParticipantForResponseDto> UpdateParticipantResponseType(Guid participantId, ParticipantForUpdateResponseTypeDto dto)
    {
        Participant participant = await _participantRepository.GetParticipantWithParticipantId(participantId) ?? throw new ParticipantNotFoundException();
        participant = _mapper.MapToParticipantFromUpdateResponseTypeDto(participant, dto);
        await _participantRepository.UpdateParticipant(participantId, participant);

        return _mapper.Map<ParticipantForResponseDto>(participant);
    }

    public async Task<bool> DeleteParticipant(Guid participantId)
    {
        await _participantRepository.DeleteParticipant(participantId);

        return true;
    }

    public async Task<ParticipantForResponseDto> GetParticipant(Guid userId, Guid eventId)
    {
        Event participantsEvent = await _eventRepository.GetEventForUser(userId, eventId);
        Participant? participant = participantsEvent.Participants
            .Where(p => p.UserId == userId)
            .FirstOrDefault();

        if (participant is null)
        {
            throw new ParticipantNotFoundInEventException(userId, eventId);
        }

        return _mapper.Map<ParticipantForResponseDto>(participant);
    }

    public async Task<IEnumerable<ParticipantForResponseDto>> GetAllParticipantsForEvent(Guid userId, Guid eventId)
    {
        Event participantsEvent = await _eventRepository.GetEventForUser(userId, eventId);
        IEnumerable<Participant> participants = [.. participantsEvent.Participants];

        return _mapper.Map<IEnumerable<ParticipantForResponseDto>>(participants); ;
    }

    public async Task<IEnumerable<ParticipantForResponseDto>> GetAllParticipantsForUser(Guid userId)
    {
        IEnumerable<Participant> participants = await _participantRepository.GetAllParticipantsForUser(userId);

        return _mapper.Map<IEnumerable<ParticipantForResponseDto>>(participants);
    }
}
