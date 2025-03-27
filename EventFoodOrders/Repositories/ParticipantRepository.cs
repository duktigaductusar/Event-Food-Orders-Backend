using EventFoodOrders.Data;
using EventFoodOrders.Exceptions;
using EventFoodOrders.Entities;
using Microsoft.EntityFrameworkCore;
using EventFoodOrders.Repositories.Interfaces;

namespace EventFoodOrders.Repositories;

public class ParticipantRepository(IDbContextFactory<EventFoodOrdersDbContext> contextFactory) :
    RepositoryBase<Participant, ParticipantNotFoundException>, IParticipantRepository
{
    private readonly IDbContextFactory<EventFoodOrdersDbContext> _contextFactory = contextFactory;

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
            else throw new ParticipantNotFoundException(participantId);

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
            else throw new ParticipantNotFoundException(participantId);

            context.SaveChanges();
        }
    }

    public Participant? GetParticipantWithParticipantId(Guid participantId)
    {
        return GetParticipant(p => p.Id == participantId, participantId);
    }

    public Participant? GetParticipantWithUserId(Guid userId)
    {
        return GetParticipant(p => p.UserId == userId, userId);
    }

    public IEnumerable<Participant> GetAllParticipantsForUser(Guid userId)
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

    private Participant? GetParticipant(Func<Participant, bool> condition, Guid id)
    {
        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            Participant? participant = context.Participants
                .Where(condition)
                .FirstOrDefault();

            if (participant is Participant)
            {
                return participant;
            }
            return null;
        }
    }
}