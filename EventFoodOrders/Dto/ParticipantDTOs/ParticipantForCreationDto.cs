using System.ComponentModel.DataAnnotations;

namespace EventFoodOrders.Dto.ParticipantDTOs;

public class ParticipantForCreationDto
{
    public bool wantsMeal { get; set; }
    public string allergies { get; set; }

    public ParticipantForCreationDto()
    {
        if (allergies == null)
        {
            allergies = string.Empty;
        }
    }
}
