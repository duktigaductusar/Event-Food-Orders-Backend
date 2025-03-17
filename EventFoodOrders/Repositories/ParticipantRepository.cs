using EventFoodOrders.Data;
using EventFoodOrders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventFoodOrders.Repositories
{
    public class ParticipantRepository(IDbContextFactory<EventFoodOrdersDbContext> contextFactory) : RepositoryBase<Participant>(contextFactory)
    {
        private IDbContextFactory<EventFoodOrdersDbContext> _contextFactory = contextFactory;

        public Participant AddParticipant(Participant participant)
        {
            using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
            {
                context.Participants.Add(participant);
                context.SaveChanges();
            }

            return participant;
        }

        public Participant UpdateParticipant(string participantId, Participant updatedParticipant)
        {
            Guid id = Guid.Parse(participantId);

            using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
            {
                Participant? participantToUpdate = context.Participants
                    .Where(e => e.participant_id == id)
                    .FirstOrDefault();

                if (participantToUpdate is Participant)
                {
                    UpdateParticipantEntity(participantToUpdate, updatedParticipant);
                }
                else throw new NullReferenceException($"The participant with id {participantId} does not exist.");

                context.SaveChanges();
            }

            return updatedParticipant;
        }

        private void UpdateParticipantEntity(Participant source, Participant destination)
        {
            destination.participant_id = source.participant_id;
            destination.EventId = source.EventId;
            destination.wantsMeal = source.wantsMeal;
            destination.allergies = source.allergies;
            destination.user = source.user;
        }
    }
}