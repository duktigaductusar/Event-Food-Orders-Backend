﻿using EventFoodOrders.Data;
using EventFoodOrders.Exceptions;
using EventFoodOrders.Entities;
using Microsoft.EntityFrameworkCore;
using EventFoodOrders.Repositories.Interfaces;

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
        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            Event? eventToUpdate = context.Events
                .Where(e => e.Id == eventId)
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

    // Helper functions
    private static void UpdateEventEntity(Event source, Event destination)
    {
        destination.Title = source.Title;
        destination.Date = source.Date;
        destination.Description = source.Description;
        destination.Deadline = source.Deadline;
    }
}