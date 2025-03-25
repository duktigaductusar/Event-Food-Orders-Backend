using EventFoodOrders.Entities;

namespace EventFoodOrders.Repositories.Interfaces
{
    public interface IEventRepository
    {
        Event AddEvent(Event newEvent);
        void DeleteEvent(Guid eventId);
        IEnumerable<Event> GetAllEventsForUser(Guid userId);
        Event GetEventForUser(Guid userId, Guid eventId);
        Event? GetSingleEventWithCondition(Func<Event, bool> condition);
        Event UpdateEvent(Guid eventId, Event updatedEvent);
    }
}