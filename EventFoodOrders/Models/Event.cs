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
        Name = "";
    }

    public Event(Guid guid)
    {
        this.Event_id = guid;
        this.Name = "";
    }

    [Key]
    [Column("event_id")]
    [Required]
    public Guid Event_id { get; set; }


    [Required]
    [Column("event_name")]
    public string Name { get; set; }

    [Required]
    [Column("event_date")]
    public DateTime EventDate { get; set; }

    [Required]
    [Column("active")]
    public bool Active { get; set; }


}
