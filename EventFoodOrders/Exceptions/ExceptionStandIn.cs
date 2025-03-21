namespace EventFoodOrders.Exceptions;

public class ExceptionStandIn<T> where T : Exception, new()
{
    internal void ThrowDefaultException()
    {
        throw new T();
    }
}
