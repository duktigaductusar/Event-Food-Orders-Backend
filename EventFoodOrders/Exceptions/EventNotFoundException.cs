namespace EventFoodOrders.Exceptions;

public class EventNotFoundException : Exception
{
    public EventNotFoundException() :
        base("Event not found.")
    { }

    public EventNotFoundException(Guid eventId) :
        base($"Event with id {eventId} not found.")
    { }

    public EventNotFoundException(string eventId) :
        base($"Event with id {eventId} not found.")
    { }

    public EventNotFoundException(Guid eventId, Exception innerException) :
        base($"Event with id {eventId} not found.", innerException)
    { }

    public EventNotFoundException(string eventId, Exception innerException) :
        base($"Event with id {eventId} not found.", innerException)
    { }
}
