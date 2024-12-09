namespace EventFoodOrders.Dto;

public class ParticipantRegistrationRequestDTO
{
    public Guid userId { get; set; }
    public Guid eventId { get; set; }
    public bool wantsMeal { get; set; }
}
