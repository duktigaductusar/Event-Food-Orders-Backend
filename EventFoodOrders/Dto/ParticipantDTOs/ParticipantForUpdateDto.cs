namespace EventFoodOrders.Dto.ParticipantDTOs;

public class ParticipantForUpdateDto
{
    public bool wantsMeal { get; set; }
    public string allergies { get; set; }

    public ParticipantForUpdateDto()
    {
        if (allergies == null)
        {
            allergies = string.Empty;
        }
    }

}
