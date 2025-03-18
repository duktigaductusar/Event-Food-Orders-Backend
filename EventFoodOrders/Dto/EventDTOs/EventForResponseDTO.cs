namespace EventFoodOrders.Dto.EventDTOs;

public record EventForResponseDto
{
    public Guid? Id { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public required DateTimeOffset Date { get; init; }
    public required DateTimeOffset Deadline { get; init; }
    public required bool IsOwner { get; init; }
    public bool? Active { get; init; }

}