namespace EventFoodOrders.Dto;

public class ParticipantRegistrationRequest
{
    private Guid UserId { get; set; }
    private Guid EventId { get; set; }
    private bool WantsMeal { get; set; }
}
