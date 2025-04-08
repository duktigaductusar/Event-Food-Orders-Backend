using EventFoodOrders.Data;
using EventFoodOrders.Exceptions;
using EventFoodOrders.Entities;
using Microsoft.EntityFrameworkCore;
using EventFoodOrders.Repositories.Interfaces;
using System.Linq;

namespace EventFoodOrders.Repositories;

public class EventRepository(IDbContextFactory<EventFoodOrdersDbContext> contextFactory) :
    RepositoryBase<Event, EventNotFoundException>, IEventRepository
{
    private IDbContextFactory<EventFoodOrdersDbContext> _contextFactory = contextFactory;

    public Event AddEvent(Event newEvent)
    {
        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            newEvent = context.Events.Add(newEvent).Entity;
            context.SaveChanges();
        }

        return newEvent;
    }

    public Event UpdateEvent(Guid eventId, Event updatedEvent)
    {
        if (eventId == updatedEvent.Id)
        {
            using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
            {
                Event? eventToUpdate = context.Events
                    .Where(e => e.Id == eventId)
                    .Include(e => e.Participants)
                    .FirstOrDefault();

                if (eventToUpdate is not null)
                {
                    context.Entry(eventToUpdate).CurrentValues.SetValues(updatedEvent);
                    context.SaveChanges();
                    return eventToUpdate;
                }
            }
        }
        throw new EventNotFoundException(eventId);
    }

    public void DeleteEvent(Guid eventId)
    {
        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            Event? eventToUpdate = context.Events
                .Where(e => e.Id == eventId)
                .FirstOrDefault();

            if (eventToUpdate is Event)
            {
                context.Remove(eventToUpdate);
            }
            else throw new EventNotFoundException(eventId);

            context.SaveChanges();
        }
    }

    public Event GetEventForUser(Guid userId, Guid eventId)
    {
        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            Event? eventToFind = context.Events
                .Where(e => e.Id == eventId)
                .AsNoTracking()
                .Include(e => e.Participants)
                .FirstOrDefault();

            if (eventToFind is Event)
            {
                if (eventToFind.Participants.Where(p => p.UserId == userId).Any())
                {
                    return eventToFind;
                }
            }

            throw new EventNotFoundException(eventId);
        }
    }

    public IEnumerable<Event> GetAllEventsForUser(Guid userId)
    {
        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            IEnumerable<Event> events = context.Events
                .AsNoTracking()
                .Include(e => e.Participants)
                .Where(e => e.Participants.Where(p => p.UserId == userId).Count() > 0)
                .ToList();

            return events;
        }
    }

    public Event? GetSingleEventWithCondition(Func<Event, bool> condition)
    {
        Event? result;

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            result = context.Events
                .AsNoTracking()
                .Include(e => e.Participants)
                .Where(condition)
                .FirstOrDefault();
        }

        return result;
    }

    public IEnumerable<Participant> GetParticipantsByEventId(Guid eventId)
    {
        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            var participants = context.Events
                .AsNoTracking()
                .Where(e => e.Id == eventId)
                .Include(e => e.Participants)
                .SelectMany(e => e.Participants)
                .ToList();

            return participants;
        }
    }
    
    //For the reminder IHostedService
    public async Task<List<Event>> GetAllEventsAtDeadline(DateTime now)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var res = await context.Events.Where(e => e.Deadline.Date == now.Date).ToListAsync();
        return res;
    }
}