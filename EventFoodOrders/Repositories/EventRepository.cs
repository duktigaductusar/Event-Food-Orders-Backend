using EventFoodOrders.Data;
using EventFoodOrders.Exceptions;
using EventFoodOrders.Models;
using Microsoft.EntityFrameworkCore;

namespace EventFoodOrders.Repositories;

public class EventRepository(IDbContextFactory<EventFoodOrdersDbContext> contextFactory) :
    RepositoryBase<Event, EventNotFoundException>
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

    public Event UpdateEvent(string eventId, Event updatedEvent)
    {
        //ToDo: Update ID method
        Guid id = Guid.Parse(eventId);

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            Event? eventToUpdate = context.Events
                .Where(e => e.Id == id)
                .FirstOrDefault();

            if (eventToUpdate is Event)
            {
                UpdateEventEntity(updatedEvent, eventToUpdate);
            }
            else throw new EventNotFoundException(eventId);

            context.SaveChanges();
        }

        return updatedEvent;
    }

    public void DeleteEvent(string eventId)
    {
        //ToDo: Update ID method
        Guid id = Guid.Parse(eventId);

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            Event? eventToUpdate = context.Events
                .Where(e => e.Id == id)
                .FirstOrDefault();

            if (eventToUpdate is Event)
            {
                context.Remove(eventToUpdate);
            }
            else throw new EventNotFoundException(eventId);

            context.SaveChanges();
        }
    }

    public Event GetEventForUser(string userId, string eventId)
    {
        //ToDo: Update ID method
        Guid id = Guid.Parse(eventId);
        Guid uId = Guid.Parse(userId);

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            Event? eventToFind = context.Events
                .Where(e => e.Id == id)
                .AsNoTracking()
                .Include(e => e.Participants)
                .FirstOrDefault();

            if (eventToFind is Event)
            {
                if (eventToFind.Participants.Where(p => p.Id == uId).Any())
                {
                    return eventToFind;
                }
            }

            throw new EventNotFoundException(eventId);
        }
    }

    public IEnumerable<Event> GetAllEventsForUser(string userId)
    {
        //ToDo: Update ID method
        Guid id = Guid.Parse(userId);

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            IEnumerable<Event> events = context.Events
                .AsNoTracking()
                .Include(e => e.Participants)
                //.Where(e => e.id == id)
                .ToList();

            return events;
        }
    }

    internal Event GetSingleEventWithCondition(Func<Event, bool> condition)
    {
        Event result;

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            result = GetSingleWithCondition(context.Events, condition);
        }

        return result;
    }

    private static void UpdateEventEntity(Event source, Event destination)
    {
        destination.Title = source.Title;
        destination.Date = source.Date;
        destination.Description = source.Description;
        destination.Deadline = source.Deadline;
    }
}