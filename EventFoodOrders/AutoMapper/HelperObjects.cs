namespace EventFoodOrders.AutoMapper
{
    internal class EventForCreationObject
    {
        public Guid OwnerId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset Date { get; set; }
        public DateTimeOffset Deadline { get; set; }
    }

    internal class ParticipantForCreationObject
    {
        public Guid UserId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset Date { get; set; }
        public DateTimeOffset Deadline { get; set; }
    }

    internal class ParticipantForUpdateObject
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public string? Name { get; set; }
        public string? ResponseType { get; set; }
        public bool WantsMeal { get; set; }
        public string[]? Allergies { get; set; }
        public string[]? Preferences { get; set; }
    }
}
