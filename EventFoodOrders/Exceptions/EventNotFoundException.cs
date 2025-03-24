namespace EventFoodOrders.Exceptions;

public class EventNotFoundException : CustomException
{
    public EventNotFoundException() :
        base(StatusCodes.Status400BadRequest, message: "Event not found.")
    { }

    public EventNotFoundException(string message = "Event not found.") :
        base(StatusCodes.Status400BadRequest, message: message)
    { }

    public EventNotFoundException(Guid eventId) :
        this(message: $"Event with id {eventId} not found.")
    { }
}
