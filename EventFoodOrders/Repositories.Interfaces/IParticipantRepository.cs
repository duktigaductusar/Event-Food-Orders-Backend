using EventFoodOrders.Entities;

namespace EventFoodOrders.Repositories.Interfaces;

/// <summary>
/// Interacts with the Participant table in the database through the Participant DbSet in the DbContext.
/// </summary>
public interface IParticipantRepository
{
    /// <summary>
    /// Adds a participant to the database.
    /// </summary>
    /// <param name="participant"></param>
    /// <returns></returns>
    Participant AddParticipant(Participant participant);

    /// <summary>
    /// Deletes a participant in the database.
    /// </summary>
    /// <param name="participantId"></param>
    void DeleteParticipant(Guid participantId);

    /// <summary>
    /// Gets all the participants with a given user Id.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    IEnumerable<Participant> GetAllParticipantsForUser(Guid userId);

    /// <summary>
    /// Gets a participant using that participant's Id.
    /// </summary>
    /// <param name="participantId"></param>
    /// <returns></returns>
    Participant? GetParticipantWithParticipantId(Guid participantId);

    /// <summary>
    /// Gets a participant using that participant's user Id.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Participant? GetParticipantWithUserId(Guid userId);

    /// <summary>
    /// Updates a participant in the database.
    /// </summary>
    /// <param name="participantId"></param>
    /// <param name="updatedParticipant"></param>
    /// <returns></returns>
    Participant UpdateParticipant(Guid participantId, Participant updatedParticipant);
}