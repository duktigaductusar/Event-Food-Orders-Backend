using EventFoodOrders.Entities;

namespace EventFoodOrders.Services.Interfaces;

public interface IMailManager
{
    /// <summary>
    /// Coordinate mails when a event is deleted.
    /// </summary>
    /// <param name="eventToDelete"></param>
    /// <param name="ownerId"></param>
    /// <returns></returns>
    Task HandleCancelEventMails(Event eventToDelete, Guid ownerId);

    /// <summary>
    /// Coordinate mails when a new event is created.
    /// </summary>
    /// <param name="newEvent"></param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    Task HandleNewEventMails(Event newEvent, IEnumerable<Guid> userIds);

    /// <summary>
    /// Coordinate mails when an event is updated.
    /// </summary>
    /// <param name="updatedEvent"></param>
    /// <param name="participantsToSendUpdateTo"></param>
    /// <param name="participantsToSendDeleteTo"></param>
    /// <returns></returns>
    Task HandleUpdateEventMails(Event updatedEvent,
        IEnumerable<Participant> participantsToSendUpdateTo,
        IEnumerable<Participant> participantsToSendDeleteTo
    );
}