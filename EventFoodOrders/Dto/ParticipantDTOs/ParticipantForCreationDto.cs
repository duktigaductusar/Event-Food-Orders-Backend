namespace EventFoodOrders.Dto.ParticipantDTOs;

public class ParticipantForCreationDto
{
    public Guid userId { get; set; }
    public Guid eventId { get; set; }
    public bool wantsMeal { get; set; }
}
