namespace EventFoodOrders.Dto.ParticipantDTOs;

public class ParticipantForResponseDto
{
    public Guid participant_Id { get; set; }
    public Guid userId { get; set; }
    public Guid eventId { get; set; }
    public bool wantsMeal { get; set; }
    public string allergies { get; set; }
    public string responseType { get; set; }

    //ToDo: Update assignment to response dto ?
    /*
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
    */
}
