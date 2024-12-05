namespace EventFoodOrders.Exceptions
{
    [Serializable]
    internal class AccessDeniedException : Exception
    {

        public AccessDeniedException(string? message) : base(message)
        {
        }

        public AccessDeniedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

    }
}