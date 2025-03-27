using EventFoodOrders.Dto.ParticipantDTOs;
using EventFoodOrders.Entities;

namespace EventFoodOrders.Services.Interfaces;

/// <summary>
/// Handles getting, creating, and updating participants using the participant repository.
/// </summary>
public interface IParticipantService
{
    /// <summary>
    /// Adds a participant to an event, given the event's Id and a participant dto.
    /// Checks whether there is a previous participant with the same user id and if yes fetches allergy and food preferences from it.
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="newParticipant"></param>
    /// <returns></returns>
    ParticipantForResponseDto AddParticipantToEvent(Guid eventId, ParticipantForCreationDto newParticipant);

    /// <summary>
    /// Creates a participant and adds a reference to an event.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="eventId"></param>
    /// <returns></returns>
    Participant CreateParticipant(Guid userId, Guid eventId);

    /// <summary>
    /// Deletes a participant.
    /// </summary>
    /// <param name="participantId"></param>
    /// <returns></returns>
    bool DeleteParticipant(Guid participantId);

    /// <summary>
    /// Gets all the participants in an event, given that the user is a registered participant.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="eventId"></param>
    /// <returns></returns>
    IEnumerable<ParticipantForResponseDto> GetAllParticipantsForEvent(Guid userId, Guid eventId);

    /// <summary>
    /// Gets all the participations for a user.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    IEnumerable<ParticipantForResponseDto> GetAllParticipantsForUser(Guid userId);

    /// <summary>
    /// Gets a specific participant for a user in a specific event.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="eventId"></param>
    /// <returns></returns>
    ParticipantForResponseDto GetParticipant(Guid userId, Guid eventId);

    /// <summary>
    /// Updates a participant using a dto.
    /// </summary>
    /// <param name="participantId"></param>
    /// <param name="updatedParticipantDto"></param>
    /// <returns></returns>
    ParticipantForResponseDto UpdateParticipant(Guid participantId, ParticipantForUpdateDto updatedParticipantDto);
}