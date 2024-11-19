namespace EventFoodOrders.Dto;

public class ParticipantDTO
{
    private Guid participantId { get; set; }
    private Guid userId { get; set; }
    private Guid eventId { get; set; }
    private DateTime EventDate { get; set; }
    private bool WantsMeal { get; set; }
    private String Allergies { get; set; }

    public ParticipantDTO(Guid participantId, Guid userId, Guid eventId, DateTime eventDate, bool wantsMeal, string allergies)
    {

        this.participantId = participantId;
        this.userId = userId;
        this.eventId = eventId;
        EventDate = eventDate;
        WantsMeal = wantsMeal;
        Allergies = allergies;

        if (Allergies == null)
        {
            Allergies = String.Empty;
        }

    }
}
