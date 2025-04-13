namespace EventFoodOrders.IdHandling;

public interface IIdCarrier
{
    /// <summary>
    /// Carries the Id of the user currently making a request.
    /// </summary>
    Guid UserId { get; set; }
}