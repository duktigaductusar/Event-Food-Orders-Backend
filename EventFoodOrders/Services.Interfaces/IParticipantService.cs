using EventFoodOrders.Dto.ParticipantDTOs;
using EventFoodOrders.Entities;

namespace EventFoodOrders.Services.Interfaces;

public interface IParticipantService
{
    ParticipantForResponseDto AddParticipantToEvent(Guid eventId, ParticipantForCreationDto newParticipant);
    Participant CreateParticipant(Guid userId, Guid eventId);
    bool DeleteParticipant(Guid participantId);
    IEnumerable<ParticipantForResponseDto> GetAllParticipantsForEvent(Guid userId, Guid eventId);
    IEnumerable<ParticipantForResponseDto> GetAllParticipantsForUser(Guid userId);
    ParticipantForResponseDto GetParticipant(Guid userId, Guid eventId);
    ParticipantForResponseDto UpdateParticipant(Guid participantId, ParticipantForUpdateDto updatedParticipantDto);
}