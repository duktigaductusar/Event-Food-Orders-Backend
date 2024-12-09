namespace EventFoodOrders.Dto;

public class ParticipantUpdateRequestDTO
{
    private bool wantsMeal { get; set; }
    private string allergies { get; set; }

    public ParticipantUpdateRequestDTO()
    {
        if (allergies == null)
        {
            allergies = string.Empty;
        }
    }

}
