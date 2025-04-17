using EventFoodOrders.Data;
using EventFoodOrders.Exceptions;
using EventFoodOrders.Entities;
using Microsoft.EntityFrameworkCore;
using EventFoodOrders.Utilities;
using System.Linq.Expressions;
using EventFoodOrders.Repositories.Interfaces;

namespace EventFoodOrders.Repositories;

public class EventRepository(
    IDbContextFactory<EventFoodOrdersDbContext> contextFactory
) : IEventRepository
{
    private readonly IDbContextFactory<EventFoodOrdersDbContext> _contextFactory = contextFactory;

    public async Task<Event?> GetEventByIdWithParticipants(Guid eventId)
    {
        await using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Events
                .AsNoTracking()
                .Where(e => e.Id == eventId)
                .Include(e => e.Participants)
                .FirstOrDefaultAsync();
        }
    }

    public async Task<Event> AddEvent(Event newEvent)
    {
        await using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            context.Events.Add(newEvent);
            await context.SaveChangesAsync();
        }

        return newEvent;
    }

    public async Task<Event> UpdateEvent(Guid eventId, Event updatedEvent)
    {
        if (eventId != updatedEvent.Id)
        {
            throw new EventNotFoundException(eventId);
        }

        await using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            Event? eventToUpdate = await context.Events
                .Where(e => e.Id == eventId)
                .Include(e => e.Participants)
                .FirstOrDefaultAsync();

            if (eventToUpdate is null)
            {
                throw new EventNotFoundException(eventId);
            }

            context.Entry(eventToUpdate).CurrentValues.SetValues(updatedEvent);
            await context.SaveChangesAsync();
            
            return eventToUpdate;
        }
    }

    public async Task DeleteEvent(Guid userId, Guid eventId)
    {
        await using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            Event? eventToUpdate = await context.Events
                .Where(e => e.Id == eventId)
                .Where(e => e.OwnerId == userId)
                .FirstOrDefaultAsync();

            if (eventToUpdate is null)
            {
                throw new EventNotFoundException(eventId);
            }

            context.Remove(eventToUpdate);
            await context.SaveChangesAsync();
        }
    }

    public async Task<Event> GetEventForUser(Guid userId, Guid eventId)
    {
        await using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            Event? eventToFind = await context.Events
                .Where(e => e.Id == eventId)
                .AsNoTracking()
                .Include(e => e.Participants)
                .FirstOrDefaultAsync();

            if (eventToFind is not null &&
                eventToFind.Participants.Where(p => p.UserId == userId).Any())
            {
                return eventToFind;
            }

            throw new EventNotFoundException(eventId);
        }
    }

    public async Task<IEnumerable<Event>> GetAllEventsForUser(Guid userId)
    {
        await using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Events
                .AsNoTracking()
                .Include(e => e.Participants)
                .Where(e => e.Participants.Where(p => p.UserId == userId).Count() > 0)
                .ToListAsync();
        }
    }


    public async Task<IEnumerable<Participant>> GetAttendingOfficeParticipantsDescendingByUpdate(IEnumerable<Guid> userIds)
    {
        await using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Events
                .AsNoTracking()
                .Include(e => e.Participants)
                .Where(e => e.Participants.Any(p => userIds.Contains(p.UserId)))
                .SelectMany(e => e.Participants.Where(p => userIds.Contains(p.UserId)))
                .Where(p => p.ResponseType == ReType.AttendingOffice)
                .OrderByDescending(p => p.LastUpdated)
                .ToListAsync();
        }
    }

    public async Task<Event?> GetSingleEventWithCondition(Expression<Func<Event, bool>> condition)
    {
        await using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Events
                .AsNoTracking()
                .Include(e => e.Participants)
                .Where(condition)
                .FirstOrDefaultAsync();
        }
    }

    public async Task<IEnumerable<Participant>> GetParticipantsByEventId(Guid eventId)
    {
        await using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Events
                .AsNoTracking()
                .Where(e => e.Id == eventId)
                .Include(e => e.Participants)
                .SelectMany(e => e.Participants)
                .ToListAsync();
        }
    }

    //For the reminder IHostedService
    public async Task<List<Event>> GetAllEventsAtDeadline(DateTime now)
    {
        await using (var context = await _contextFactory.CreateDbContextAsync())
        {
            return await context.Events
                .Where(e => e.Deadline.Date == now.Date)
                .ToListAsync();
        }
    }

    // For the summary IHostedService
    public async Task<IEnumerable<Event>> GetEventsWithPassedDeadlines(int? nrOfDaysBeforeDeadline)
    {
        await using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Events
                .Where(e =>
                    e.Deadline < DateTimeOffset.UtcNow.AddDays(nrOfDaysBeforeDeadline ?? 0) &&
                    e.Status == EventStatus.BeforeDeadline)
                .OrderBy(e => e.Deadline)
                .Include(e => e.Participants)
                .ToListAsync();
        }
    }

    // For the summary IHostedService
    public async Task UpdateEventToStatusDeadlinePassed(Event focusedEvent)
    {
        await using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            var findEvent = await context.Events
                .Where(e => e.Id == focusedEvent.Id)
                .FirstOrDefaultAsync();

            if (findEvent == null)
            {
                return;
            }

            findEvent.Status = EventStatus.DeadlinePassed;

            await context.SaveChangesAsync();
        }
    }
}