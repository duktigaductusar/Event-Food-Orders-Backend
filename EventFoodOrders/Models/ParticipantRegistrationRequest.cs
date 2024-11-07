namespace EventFoodOrders.Models;

public class ParticipantRegistrationRequest
{
    public Guid UserId { get; set; }
    public Guid EventId { get; set; }
    public bool WantsMeal { get; set; }
    public string Allergies { get; set; }


}
