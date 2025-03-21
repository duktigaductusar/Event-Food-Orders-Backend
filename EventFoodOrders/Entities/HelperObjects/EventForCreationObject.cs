namespace EventFoodOrders.Entities.HelperModels
{
    public class EventForCreationObject
    {
        public Guid UserId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset Date { get; set; }
        public DateTimeOffset Deadline { get; set; }
    }
}
