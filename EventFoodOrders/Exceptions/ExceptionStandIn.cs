namespace EventFoodOrders.Exceptions;

public class ExceptionStandIn<T> where T : CustomException, new()
{
    internal void ThrowDefaultException()
    {
        throw new T();
    }
}
