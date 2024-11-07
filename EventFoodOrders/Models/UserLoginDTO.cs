namespace EventFoodOrders.Models;

public class UserLoginDTO
{
    //@NotBlank(message = "Email is required")
    //@Email(message = "Invalid email format")
    //@Schema(
    //        description = "User's email address",
    //        example = "user@example.com"
    //)
    private string email { get; set; }

    //@NotBlank(message = "Password is required")
    //@Schema(
    //        description = "User's password",
    //        example = "password123"
    //)
    private string password { get; set; }
}
