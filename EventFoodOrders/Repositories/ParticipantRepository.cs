using EventFoodOrders.Data;
using EventFoodOrders.Exceptions;
using EventFoodOrders.Models;
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

    public Participant UpdateParticipant(string participantId, Participant updatedParticipant)
    {
        Guid id = Guid.Parse(participantId);

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            Participant? participantToUpdate = context.Participants
                .Where(e => e.Id == id)
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

    public void DeleteParticipant(string participantId)
    {
        Guid id = Guid.Parse(participantId);

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            Participant? participantToUpdate = context.Participants
                .Where(e => e.Id == id)
                .FirstOrDefault();

            if (participantToUpdate is Participant)
            {
                context.Remove(participantToUpdate);
            }
            else throw new NullReferenceException($"A participant with id {participantId} does not exist.");

            context.SaveChanges();
        }
    }

    internal Participant GetParticipant(string participantId)
    {
        Guid id = Guid.Parse(participantId);

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            Participant? participant = context.Participants
                .Where(e => e.Id == id)
                .AsNoTracking()
                .FirstOrDefault();

            if (participant is Participant)
            {
                return participant;
            }
            else throw new NullReferenceException($"A participant with id {participantId} does not exist.");
        }
    }

    private void UpdateParticipantEntity(Participant destination, Participant source)
    {
        destination.Id = source.Id;
        destination.EventId = source.EventId;
        destination.WantsMeal = source.WantsMeal;
        destination.Allergies = source.Allergies;
        destination.Preferences = source.Preferences;
    }
}