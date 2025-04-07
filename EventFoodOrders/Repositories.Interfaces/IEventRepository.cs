using EventFoodOrders.Entities;

namespace EventFoodOrders.Repositories.Interfaces;

/// <summary>
/// Interacts with the Event table in the database through the Event DbSet in the DbContext.
/// </summary>
public interface IEventRepository
{
    /// <summary>
    /// Adds an event to the database.
    /// </summary>
    /// <param name="newEvent"></param>
    /// <returns></returns>
    Event AddEvent(Event newEvent);
    
    /// <summary>
    /// Deletes an event in the database.
    /// </summary>
    /// <param name="eventId"></param>
    void DeleteEvent(Guid eventId);

    /// <summary>
    /// Gets all the events in which the user is a participant.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    IEnumerable<Event> GetAllEventsForUser(Guid userId);

    /// <summary>
    /// Gets a single event, given that the user is a participant.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="eventId"></param>
    /// <returns></returns>
    Event GetEventForUser(Guid userId, Guid eventId);

    /// <summary>
    /// Gets a single event given a condition.
    /// </summary>
    /// <param name="condition"></param>
    /// <returns></returns>
    Event? GetSingleEventWithCondition(Func<Event, bool> condition);

    /// <summary>
    /// Updates an event in the database.
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="updatedEvent"></param>
    /// <returns></returns>
    Event UpdateEvent(Guid eventId, Event updatedEvent);

    /// <summary>
    /// Get all events which have a deadline that is on the DateTime.date sent as parameter.
    /// </summary>
    /// <param name="now"></param>
    /// <returns></returns>
    Task<List<Event>> GetAllEventsAtDeadline(DateTime now);
}