namespace EventFoodOrders.Dto;

public class ParticipantDTO
{
    private Guid participantId { get; set; }
    private Guid userId { get; set; }
    private Guid eventId { get; set; }
    private DateTime EventDate { get; set; }
    private bool wantsMeal { get; set; }
    private String allergies { get; set; }

    public ParticipantDTO(Guid participantId, Guid userId, Guid eventId, DateTime eventDate, bool wantsMeal, string Allergies)
    {

        this.participantId = participantId;
        this.userId = userId;
        this.eventId = eventId;
        EventDate = eventDate;
        this.wantsMeal = wantsMeal;
        allergies = Allergies;

        if (allergies == null)
        {
            allergies = string.Empty;
        }

    }
}
