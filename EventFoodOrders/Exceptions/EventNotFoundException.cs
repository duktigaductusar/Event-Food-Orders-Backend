namespace EventFoodOrders.Exceptions;

public class EventNotFoundException : Exception
{
    private EventNotFoundException() { }

    public EventNotFoundException(string message) : base(message) { }

    public EventNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}
