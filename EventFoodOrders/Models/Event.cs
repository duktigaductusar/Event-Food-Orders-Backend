
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventFoodOrders.Models;

[Table("events")]
public class Event
{
    public Event()
    {
        if (Event_id == Guid.Empty)
        {
            Event_id = Guid.NewGuid();
        }
        Active = false;
    }

    [Key]
    [Column("event_id")]
    [Required]
    private Guid Event_id { get; }


    [Required]
    [Column("event_date")]
    private DateAndTime EventDate { get; set; }

    [Required]
    [Column("active")]
    private bool Active { get; set; }


}
