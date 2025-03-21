namespace EventFoodOrders.Dto.ParticipantDTOs;

public class ParticipantForUpdateDto
{
    public bool? wantsMeal { get; set; }
    public string? allergies { get; set; }
    public string? responseType { get; set; }
}
