using System.ComponentModel.DataAnnotations;

namespace EventFoodOrders.Options;

public class AppGraphOptions
{
    [Required]
    public string SenderEmail { get; set; } = default!;
}
