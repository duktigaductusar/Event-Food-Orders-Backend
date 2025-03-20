namespace EventFoodOrders.Dto.EventDTOs;

public record EventForResponseWithDetailsDto
{
    // Event properties
    public required Guid? Id { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public required DateTimeOffset Date { get; init; }
    public required DateTimeOffset Deadline { get; init; }
    public bool? IsActive { get; init; }

    // Participant properties
    public required string ParticipantID { get; set; }
    public required bool IsOwner { get; set; }
    public required string ParticipantResponseType { get; set; }
    public required bool WantsMeal { get; set; }
    public required string[] Allergies { get; set; }
    public required string[] Preferences { get; set; }
}
