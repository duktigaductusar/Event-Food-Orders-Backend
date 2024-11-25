namespace EventFoodOrders.Dto;

public class ErrorDTO
{
    private String Message { get; set; }
    private String Details { get; set; }

    public ErrorDTO(String message, String details)
    {
        this.Message = message;
        this.Details = details;
    }

}
