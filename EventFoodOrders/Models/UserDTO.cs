namespace EventFoodOrders.Models;

public class UserDTO
{
    //@Schema(description = "User's name", example = "Duck Ductus")
    private String name;
    //@Schema(description = "User's email address", example = "user@example.com")
    private String email;
    //@Schema(description = "User's password", example = "password123")
    private String password;
    //@Schema(description = "User's role", example = "ADMIN")
    private Role role;
}
