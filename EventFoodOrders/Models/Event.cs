using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventFoodOrders.Models;

[Table("events")]
public class Event
{
    public Event()
    {
        if (id == Guid.Empty)
        {
            id = Guid.NewGuid();
        }
        Active = false;
        EventName = "";
    }

    public Event(Guid guid)
    {
        this.id = guid;
        this.EventName = "";
    }

    [Key]
    [Column("event_id")]
    [Required]
    public Guid id { get; set; }


    [Required]
    [Column("event_name")]
    public string EventName { get; set; }

    [Required]
    [Column("event_date")]
    public DateTimeOffset EventDate { get; set; }

    [Required]
    [Column("active")]
    public bool Active { get; set; }

    [Column("description")]
    public string? description { get; set; }

    public override string? ToString()
    {
        return base.ToString();
    }
}
