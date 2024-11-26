using EventFoodOrders.Models;

namespace EventFoodOrders.Dto;

public class UserDTO
{
    private string Name { get; set; }
    private string Email { get; set; }
    private string Allergies { get; set; }
    private Role Role { get; set; }

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

    public string getEmail()
    {
        return Email;
    }

    public string getName()
    {
        return Name;
    }

}

