namespace EventFoodOrders.Exceptions
{
    public class EventAlreadyExistsException : Exception
    {
        private EventAlreadyExistsException() { }

        public EventAlreadyExistsException(string message) : base(message) { }

        public EventAlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }
    }
}
