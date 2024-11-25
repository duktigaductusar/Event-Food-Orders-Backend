namespace EventFoodOrders.Dto;

public class UserLoginDTO
{
    private String email { get; set; }

    public UserLoginDTO()
    {
        if (email == null)
        {
            email = string.Empty;
        }
    }
}
