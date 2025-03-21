namespace EventFoodOrders.Dto.ParticipantDTOs;

public class ParticipantForCreationDto
{
    public bool WantsMeal { get; set; }
    public string[] Allergies { get; set; }

    public ParticipantForCreationDto()
    {
        if (Allergies == null)
        {
            Allergies = [];
        }
    }
}
