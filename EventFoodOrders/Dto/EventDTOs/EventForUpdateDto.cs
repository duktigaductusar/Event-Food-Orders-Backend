namespace EventFoodOrders.Dto.EventDTOs
{
    public record EventForUpdateDto
    {
        public Guid EventId { get; set; }
        public string EventName { get; set; }
        public DateTimeOffset EventDate { get; set; }
        public string Description { get; set; }
        public bool EventActive { get; set; }
    }
}
