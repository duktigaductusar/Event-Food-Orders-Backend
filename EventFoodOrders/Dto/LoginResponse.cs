namespace EventFoodOrders.Dto;

public class LoginResponse
{
    public LoginResponse(string email, string message)
    {
        this.email = email;
        this.message = message;
    }

    public String email { get; set; }
    public String message { get; set; }
}