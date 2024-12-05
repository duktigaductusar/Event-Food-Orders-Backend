namespace EventFoodOrders.Exceptions;

public class EmailAlreadyExistsException : Exception
{
    public EmailAlreadyExistsException(String message) : base(message) { }

    public EmailAlreadyExistsException(String message, Exception? ex) : base(message, ex) { }

}
