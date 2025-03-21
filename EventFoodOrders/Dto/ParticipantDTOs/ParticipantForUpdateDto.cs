namespace EventFoodOrders.Dto.ParticipantDTOs;

public class ParticipantForUpdateDto
{
    public bool? WantsMeal { get; set; }
    public string? Allergies { get; set; }
    public string? ResponseType { get; set; }
}
