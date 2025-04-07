namespace EventFoodOrders.Dto.EventDTOs;

public record EventForCreationDto
{
    public required string Title { get; init; }
    public string? Description { get; init; }
    public required DateTimeOffset Date { get; init; }
    public required DateTimeOffset EndTime{ get; init; }
    public required DateTimeOffset Deadline{ get; init; }
    public Guid[]? UserIds { get; init; }
}
