namespace EventFoodOrders.Dto.ParticipantDTOs;

public class ParticipantForResponseDto
{
    private Guid participantId { get; set; }
    private Guid userId { get; set; }
    private Guid eventId { get; set; }
    private DateTime EventDate { get; set; }
    private bool wantsMeal { get; set; }
    private string allergies { get; set; }

    public ParticipantForResponseDto(Guid participantId, Guid userId, Guid eventId, DateTime eventDate, bool wantsMeal, string Allergies)
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
