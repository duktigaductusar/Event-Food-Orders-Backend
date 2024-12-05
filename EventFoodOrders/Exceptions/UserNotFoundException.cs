namespace EventFoodOrders.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(String message) : base(message) { }
    public UserNotFoundException(String message, Exception? ex) : base(message, ex) { }
}
