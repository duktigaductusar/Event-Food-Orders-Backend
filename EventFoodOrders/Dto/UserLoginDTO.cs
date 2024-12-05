namespace EventFoodOrders.Dto;

public class UserLoginDTO
{
    public String email { get; set; }

    public UserLoginDTO()
    {
        if (email == null)
        {
            email = string.Empty;
        }
    }
}
