﻿using EventFoodOrders.Data;
using EventFoodOrders.Models;
using Microsoft.EntityFrameworkCore;

namespace EventFoodOrders.Repositories
{
    public class EventRepository(IDbContextFactory<EventFoodOrdersDbContext> contextFactory) : RepositoryBase<Event>()
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
                    .Where(e => e.EventId == id)
                    .FirstOrDefault();

                if (eventToUpdate is Event)
                {
                    UpdateEventEntity(updatedEvent, eventToUpdate);
                }
                else throw new NullReferenceException($"An event with id {eventId} does not exist.");

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
                    .Where(e => e.EventId == id)
                    .FirstOrDefault();

                if (eventToUpdate is Event)
                {
                    context.Remove(eventToUpdate);
                }
                else throw new NullReferenceException($"An event with id {eventId} does not exist.");

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
                    .Where(e => e.EventId == id)
                    .AsNoTracking()
                    .Include(e => e.Participants)
                    .FirstOrDefault();

                if (eventToFind is Event)
                {
                    if (eventToFind.Participants.Where(p => p.participant_id == uId).Any())
                    {
                        return eventToFind;
                    }
                }
                
                throw new NullReferenceException($"An event with id {eventId} does not exist.");
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

                if (events.Any())
                {
                    return events;
                }
                else throw new NullReferenceException($"No events were found.");
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
            destination.EventName = source.EventName;
            destination.EventDate = source.EventDate;
            destination.Description = source.Description;
            destination.EventActive = source.EventActive;
        }
    }
}