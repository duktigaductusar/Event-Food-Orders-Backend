namespace EventFoodOrders.Exceptions;

public class IllegalArgumentException : Exception
{
    public IllegalArgumentException(String message) : base(message) { }

    public IllegalArgumentException(String message, Exception? ex) : base(message, ex) { }

}
