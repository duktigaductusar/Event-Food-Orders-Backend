namespace EventFoodOrders.Dto;

public class ParticipantUpdateRequestDTO
{
    public bool wantsMeal { get; set; }
    public string allergies { get; set; }

    public ParticipantUpdateRequestDTO()
    {
        if (allergies == null)
        {
            allergies = string.Empty;
        }
    }

}
