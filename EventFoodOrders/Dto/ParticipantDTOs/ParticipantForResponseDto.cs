namespace EventFoodOrders.Dto.ParticipantDTOs;

public class ParticipantForResponseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid EventId { get; set; }
    public bool WantsMeal { get; set; }
    public string Allergies { get; set; }
    public string Preferences { get; set; }
    public string ResponseType { get; set; }
}
