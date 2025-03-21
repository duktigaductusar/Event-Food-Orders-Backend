namespace EventFoodOrders.Dto.EventDTOs;

public class EventForResponseDto
{
    // Event properties
    public required Guid? Id { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public required DateTimeOffset Date { get; init; }
    public bool? IsActive { get; init; }

    // Participant properties
    public required bool IsOwner { get; set; }
    public required string ResponseType { get; set; }
}
