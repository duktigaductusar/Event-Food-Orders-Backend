using EventFoodOrders.Models;

namespace EventFoodOrders.Dto;

public class UserDTO
{
    public string Name { get; }
    public string Email { get; }
    public string Allergies { get; }
    public Role Role { get; }

    public UserDTO()
    {

        if (Name == null)
        {
            Name = string.Empty;
        }
        if (Email == null)
        {
            Email = string.Empty;
        }
        if (Allergies == null)
        {
            Allergies = string.Empty;
        }
    }
}

