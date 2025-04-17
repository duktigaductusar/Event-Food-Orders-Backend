using EventFoodOrders.Data;
using EventFoodOrders.Exceptions;
using EventFoodOrders.Entities;
using Microsoft.EntityFrameworkCore;
using EventFoodOrders.Repositories.Interfaces;
using System.Linq.Expressions;

namespace EventFoodOrders.Repositories;

public class ParticipantRepository(
    IDbContextFactory<EventFoodOrdersDbContext> contextFactory
) : IParticipantRepository
{
    private readonly IDbContextFactory<EventFoodOrdersDbContext> _contextFactory = contextFactory;

    public async Task<Participant> AddParticipant(Participant participant)
    {
        await using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            context.Participants.Add(participant);
            await context.SaveChangesAsync();
        }

        return participant;
    }

    public async Task<IEnumerable<Participant>> AddParticipants(IEnumerable<Participant> participants)
    {
        await using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            context.Participants.AddRange(participants);
            await context.SaveChangesAsync();
        }

        return participants;
    }

    public async Task<Participant> UpdateParticipant(Guid participantId, Participant updatedParticipant)
    {
        await using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            Participant? participantToUpdate = context.Participants
                .Where(e => e.Id == participantId)
                .FirstOrDefault();

            if (participantToUpdate is Participant)
            {
                UpdateParticipantEntity(participantToUpdate, updatedParticipant);
            }
            else throw new ParticipantNotFoundException(participantId);

            await context.SaveChangesAsync();
        }

        return updatedParticipant;
    }

    public async Task DeleteParticipant(Guid participantId)
    {
        await using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            Participant? participantToUpdate = await context.Participants
                .Where(e => e.Id == participantId)
                .FirstOrDefaultAsync();

            if (participantToUpdate is Participant)
            {
                context.Remove(participantToUpdate);
            }
            else throw new ParticipantNotFoundException(participantId);

            await context.SaveChangesAsync();
        }
    }

    public async Task<Participant?> GetParticipantWithParticipantId(Guid participantId)
    {
        return await GetParticipantAsync(p => p.Id == participantId);
    }

    public async Task<Participant?> GetParticipantWithEventAndUserId(Guid eventId, Guid userId)
    {
        return await GetParticipantAsync(p => p.EventId == eventId && p.UserId == userId);
    }

    public async Task<IEnumerable<Participant>> GetAllParticipantsForUser(Guid userId)
    {
        await using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            IEnumerable<Participant> participants = await context.Participants
                .Where(p => p.UserId == userId)
                .AsNoTracking()
                .ToListAsync();

            return participants;
        }
    }

    // Helper functions
    private void UpdateParticipantEntity(Participant destination, Participant source)
    {
        destination.WantsMeal = source.WantsMeal;
        destination.Allergies = source.Allergies;
        destination.Preferences = source.Preferences;
        destination.ResponseType = source.ResponseType;
    }

    private async Task<Participant?> GetParticipantAsync(Expression<Func<Participant, bool>> condition)
    {
        await using (var context = _contextFactory.CreateDbContext())
        {
            return await context.Participants
            .Where(condition)
            .FirstOrDefaultAsync();
        }
    }

}