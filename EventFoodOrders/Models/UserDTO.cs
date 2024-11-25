namespace EventFoodOrders.Models;

public class UserDTO
{
    //@Schema(description = "User's name", example = "Duck Ductus")
    public String Name;
    //@Schema(description = "User's email address", example = "user@example.com")
    public String Email;
    //@Schema(description = "User's password", example = "password123")
    public String Password;
    //@Schema(description = "User's role", example = "ADMIN")
    public Role Role;
}
