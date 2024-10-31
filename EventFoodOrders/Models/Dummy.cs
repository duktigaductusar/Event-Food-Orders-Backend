using System.ComponentModel.DataAnnotations;

namespace EventFoodOrders.Models;


public class Dummy
{
    //private string _id; //UUID
    //private string? _name;
    //private string? _description;

    [Key]
    public string Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
