using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventFoodOrders.Models;

[Table("events")]
public class Event
{
    private Guid guid;

    public Event()
    {
        if (Event_id == Guid.Empty)
        {
            Event_id = Guid.NewGuid();
        }
        Active = false;
    }

    public Event(Guid guid)
    {
        this.guid = guid;
    }

    [Key]
    [Column("event_id")]
    [Required]
    private Guid Event_id { get; }

    public Guid id
    { get { return Event_id; } }


    [Required]
    [Column("event_name")]
    private string Name;

    [Required]
    [Column("event_date")]
    private DateTime EventDate { get; set; }

    [Required]
    [Column("active")]
    private bool Active { get; set; }


}
