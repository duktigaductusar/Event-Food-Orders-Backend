namespace EventFoodOrders.Exceptions
{
    public class UnauthorizedUserException : CustomException
    {
        public UnauthorizedUserException() :
            base(StatusCodes.Status401Unauthorized, message: "User not authenticated.")
        { }
    }
}
