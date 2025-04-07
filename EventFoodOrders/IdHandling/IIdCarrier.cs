namespace EventFoodOrders.IdHandling
{
    public interface IIdCarrier
    {
        Guid UserId { get; set; }
    }
}