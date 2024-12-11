namespace EventFoodOrders.Exceptions
{
    public class BadRequestException
        : Exception
    {
        public BadRequestException(String message) : base(message) { }
        public BadRequestException(String message, Exception? ex) : base(message, ex) { }
    }

}

