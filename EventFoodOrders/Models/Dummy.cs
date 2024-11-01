using System.ComponentModel.DataAnnotations;

namespace EventFoodOrders.Models;


public class Dummy
{
    [Key]
    public string Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
