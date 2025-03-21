namespace EventFoodOrders.Dto.ParticipantDTOs;

public class ParticipantForUpdateDto
{
    public string? ResponseType { get; set; }
    public bool? WantsMeal { get; set; }
    public string[]? Allergies { get; set; }
    public string[]? Preferences { get; set; }
}
