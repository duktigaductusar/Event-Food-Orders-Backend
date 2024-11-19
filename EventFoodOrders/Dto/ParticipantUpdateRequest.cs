namespace EventFoodOrders.Dto;

public class ParticipantUpdateRequest
{
    private bool wantsMeal { get; set; }
    private string allergies { get; set; }

    public ParticipantUpdateRequest()
    {
        if (allergies == null)
        {
            allergies = string.Empty;
        }
    }

}
