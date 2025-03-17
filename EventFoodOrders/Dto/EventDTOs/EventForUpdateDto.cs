namespace EventFoodOrders.Dto.EventDTOs
{
    public class EventForUpdateDto
    {
        public Guid eventId { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
        public bool Action { get; set; }
    }
}
