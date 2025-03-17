namespace EventFoodOrders.Dto.EventDTOs
{
    public record EventForCreationDto
    {
        public string EventName { get; init; }
        public DateTimeOffset EventDate { get; init; }
        public string Description { get; init; }
        public bool EventActive { get; init; }
    }
}
