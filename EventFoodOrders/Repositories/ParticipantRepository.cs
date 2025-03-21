using EventFoodOrders.Data;
using EventFoodOrders.Exceptions;
using EventFoodOrders.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventFoodOrders.Repositories;

public class ParticipantRepository(IDbContextFactory<EventFoodOrdersDbContext> contextFactory) :
    RepositoryBase<Participant, ParticipantNotFoundException>
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

    public Participant UpdateParticipant(Guid participantId, Participant updatedParticipant)
    {
        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            Participant? participantToUpdate = context.Participants
                .Where(e => e.Id == participantId)
                .FirstOrDefault();

            if (participantToUpdate is Participant)
            {
                UpdateParticipantEntity(participantToUpdate, updatedParticipant);
            }
            else throw new NullReferenceException($"A participant with id {participantId} does not exist.");

            context.SaveChanges();
        }

        return updatedParticipant;
    }

    public void DeleteParticipant(Guid participantId)
    {
        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            Participant? participantToUpdate = context.Participants
                .Where(e => e.Id == participantId)
                .FirstOrDefault();

            if (participantToUpdate is Participant)
            {
                context.Remove(participantToUpdate);
            }
            else throw new NullReferenceException($"A participant with id {participantId} does not exist.");

            context.SaveChanges();
        }
    }

    internal Participant GetParticipant(Guid participantId)
    {
        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            Participant? participant = context.Participants
                .Where(p => p.Id == participantId)
                .AsNoTracking()
                .FirstOrDefault();

            if (participant is Participant)
            {
                return participant;
            }
            else throw new NullReferenceException($"A participant with id {participantId} does not exist.");
        }
    }

    internal IEnumerable<Participant> GetAllParticipantsForUser(Guid userId)
    {
        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            IEnumerable<Participant> participants = context.Participants
                .Where(p => p.UserId == userId)
                .AsNoTracking()
                .ToList();

            return participants;
        }
    }

    // Helper functions
    private void UpdateParticipantEntity(Participant destination, Participant source)
    {
        destination.WantsMeal = source.WantsMeal;
        destination.Allergies = source.Allergies;
        destination.Preferences = source.Preferences;
    }
}