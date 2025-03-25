using EventFoodOrders.Entities;

namespace EventFoodOrders.Repositories.Interfaces
{
    public interface IParticipantRepository
    {
        Participant AddParticipant(Participant participant);
        void DeleteParticipant(Guid participantId);
        IEnumerable<Participant> GetAllParticipantsForUser(Guid userId);
        Participant? GetParticipantWithParticipantId(Guid participantId);
        Participant? GetParticipantWithUserId(Guid userId);
        Participant UpdateParticipant(Guid participantId, Participant updatedParticipant);
    }
}