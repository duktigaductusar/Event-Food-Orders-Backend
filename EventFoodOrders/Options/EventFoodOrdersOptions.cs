using System.ComponentModel.DataAnnotations;

namespace EventFoodOrders.Options;

public class EventFoodOrdersOptions
{
    [Required]
    public string ClientBaseUrl { get; set; } = default!;

    [Required]
    public int PollingIntervalSummaryService { get; set; }
}
