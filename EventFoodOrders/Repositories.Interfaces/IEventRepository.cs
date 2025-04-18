using EventFoodOrders.Entities;
using System.Linq.Expressions;

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
    Task<Event> AddEvent(Event newEvent);

    /// <summary>
    /// Deletes an event in the database.
    /// </summary>
    /// <param name="eventId"></param>
    Task DeleteEvent(Guid userId, Guid eventId);

    /// <summary>
    /// Gets all the events in which the user is a participant.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<IEnumerable<Event>> GetAllEventsForUser(Guid userId);

    /// <summary>
    /// Gets a single event, given that the user is a participant.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="eventId"></param>
    /// <returns></returns>
    Task<Event> GetEventForUser(Guid userId, Guid eventId);


    /// <summary>
    /// Get participants by event id;
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns></returns>
    Task<IEnumerable<Participant>> GetParticipantsByEventId(Guid eventId);

    /// <summary>
    /// Gets a single event given a condition.
    /// </summary>
    /// <param name="condition"></param>
    /// <returns></returns>
    Task<Event?> GetSingleEventWithCondition(Expression<Func<Event, bool>> condition);

    /// <summary>
    /// Updates an event in the database.
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="updatedEvent"></param>
    /// <returns></returns>
    Task<Event> UpdateEvent(Guid eventId, Event updatedEvent);

    /// <summary>
    /// Get all events which have a deadline that is on the DateTime.date sent as parameter.
    /// </summary>
    /// <param name="now"></param>
    /// <returns></returns>
    Task<List<Event>> GetAllEventsAtDeadline(DateTimeOffset dateTimeOffset);

    /// <summary>
    /// Get the next upcoming deadline, based on DateTime.Now.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Event>> GetActiveEventsWithPassedDeadlines();

    /// <summary>
    /// Get ordered attending office participants by user IDs.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="participants"></param>
    /// <returns></returns>
    Task<IEnumerable<Participant>> GetAttendingOfficeParticipantsDescendingByUpdate(IEnumerable<Guid> userIds);
    
    /// <summary>
    /// Get an event by id.
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns></returns>
    Task<Event?> GetEventByIdWithParticipants(Guid eventId);

    /// <summary>
    /// Update an event to status deadline passed.
    /// </summary>
    /// <param name="focusedEvent"></param>
    /// <returns></returns>
    Task UpdateEventToStatusDeadlinePassed(Event focusedEvent);
}