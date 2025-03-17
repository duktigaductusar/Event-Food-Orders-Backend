using EventFoodOrders.Data;
using EventFoodOrders.Models;
using Microsoft.EntityFrameworkCore;

namespace EventFoodOrders.Repositories
{
    public class EventRepository(IDbContextFactory<EventFoodOrdersDbContext> contextFactory)
    {
        private IDbContextFactory<EventFoodOrdersDbContext> _contextFactory = contextFactory;

        public Event CreateEvent(Event newEvent)
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
            Guid id = Guid.Parse(eventId);

            using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
            {
                Event? eventToUpdate = context.Events
                    .Where(e => e.id == id)
                    .FirstOrDefault();

                if (eventToUpdate is Event)
                {
                    eventToUpdate = updatedEvent;
                }
                else throw new NullReferenceException($"The event with id {eventId} does not exist.");

                context.SaveChanges();
            }

            return updatedEvent;
        }

        public void DeleteEvent(string eventId)
        {
            Guid id = Guid.Parse(eventId);

            using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
            {
                Event? eventToUpdate = context.Events
                    .Where(e => e.id == id)
                    .FirstOrDefault();

                if (eventToUpdate is Event)
                {
                    context.Remove(eventToUpdate);
                }
                else throw new NullReferenceException($"The event with id {eventId} does not exist.");

                context.SaveChanges();
            }
        }

        public Event GetEventForUser(string userId, string eventId)
        {
            Guid id = Guid.Parse(eventId);

            using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
            {
                Event? eventToFind = context.Events
                    .Where(e => e.id == id)
                    .FirstOrDefault();

                if (eventToFind is Event)
                {
                    return eventToFind;
                }
                else throw new NullReferenceException($"The event with id {eventId} does not exist.");
            }
        }

        public IEnumerable<Event> GetAllEventsForUser(string userId)
        {
            Guid id = Guid.Parse(userId);

            using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Event> events = context.Events
                    //.Where(e => e.id == id)
                    .ToList();

                if (events.Any())
                {
                    return events;
                }
                else throw new NullReferenceException($"No events were found.");
            }
        }
    }
}