namespace EventFoodOrders.Dto;

public class ParticipantRegistrationRequest
{
    public Guid userId { get; set; }
    public Guid eventId { get; set; }
    public bool wantsMeal { get; set; }
}
