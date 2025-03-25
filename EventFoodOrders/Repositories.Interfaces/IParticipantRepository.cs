using EventFoodOrders.Entities;

namespace EventFoodOrders.Repositories.Interfaces;

public interface IParticipantRepository
{
    Participant AddParticipant(Participant participant);
    void DeleteParticipant(Guid participantId);
    Participant UpdateParticipant(Guid participantId, Participant updatedParticipant);
}