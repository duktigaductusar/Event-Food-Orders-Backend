namespace EventFoodOrders.Models;

public class LoginResponse
{
    //    @Schema(
    //        description = "JWT token for authentication",
    //        example = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
    //)
    private string token { get; set; }

    //@Schema(
    //        description = "Token expiration time in seconds",
    //        example = "3600"
    //)
    private long expiresIn { get; set; }

    public LoginResponse setToken(String token)
    {
        this.token = token;
        return this;
    }

    public LoginResponse setExpiresIn(long expiresIn)
    {
        this.expiresIn = expiresIn;
        return this;
    }
}
