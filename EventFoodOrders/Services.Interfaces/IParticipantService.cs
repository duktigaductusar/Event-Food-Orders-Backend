using EventFoodOrders.Dto.ParticipantDTOs;
using EventFoodOrders.Entities;

namespace EventFoodOrders.Services.Interfaces;

/// <summary>
/// Handles getting, creating, and updating participants using the participant repository.
/// </summary>
public interface IParticipantService
{
    /// <summary>
    /// Adds participants to an event, given the event's Id and an enumerable of participants.
    /// For all participants the method checks whether there is a previous attending office participant
    /// with the same user id and if yes fetches allergy and food preferences from it.
    /// </summary>
    /// <param name="newEvent"></param>
    /// <param name="participants"></param>
    /// <returns></returns>
    Task<IEnumerable<ParticipantForResponseDto>> AddParticipantsToEvent(Event newEvent, IEnumerable<Participant> participants);

    /// <summary>
    /// Deletes a participant.
    /// </summary>
    /// <param name="participantId"></param>
    /// <returns></returns>
    Task<bool> DeleteParticipant(Guid participantId);

    /// <summary>
    /// Gets all the participants in an event, given that the user is a registered participant.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="eventId"></param>
    /// <returns></returns>
    Task<IEnumerable<ParticipantForResponseDto>> GetAllParticipantsForEvent(Guid userId, Guid eventId);

    /// <summary>
    /// Gets all the participations for a user.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<IEnumerable<ParticipantForResponseDto>> GetAllParticipantsForUser(Guid userId);

    /// <summary>
    /// Gets a specific participant for a user in a specific event.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="eventId"></param>
    /// <returns></returns>
    Task<ParticipantForResponseDto> GetParticipant(Guid userId, Guid eventId);

    /// <summary>
    /// Updates a participant using a dto.
    /// </summary>
    /// <param name="participantId"></param>
    /// <param name="updatedParticipantDto"></param>
    /// <returns></returns>
    Task<ParticipantForResponseDto> UpdateParticipant(Guid participantId, ParticipantForUpdateDto updatedParticipantDto);

    /// <summary>
    /// Updates a participant's response type.
    /// </summary>
    /// <param name="participantId"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<ParticipantForResponseDto> UpdateParticipantResponseType(Guid participantId, ParticipantForUpdateResponseTypeDto dto);
}