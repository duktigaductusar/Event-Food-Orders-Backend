namespace EventFoodOrders.Exceptions;

public class ParticipantNotFoundException : Exception
{
    private ParticipantNotFoundException() { }

    public ParticipantNotFoundException(string message) : base(message) { }

    public ParticipantNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}
