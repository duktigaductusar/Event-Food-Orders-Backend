using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventFoodOrders.Models;

[Table("events")]
public class Event
{
    public Event()
    {
        if (EventId == Guid.Empty)
        {
            EventId = Guid.NewGuid();
        }
        EventName = "";
    }

    public Event(Guid guid)
    {
        EventId = guid;
        EventName = "";
    }

    [Key]
    [Column("event_id")]
    [Required]
    public Guid EventId { get; set; }


    [Required]
    [Column("event_name")]
    public string EventName { get; set; }

    [Required]
    [Column("event_date")]
    public DateTimeOffset EventDate { get; set; }

    [Required]
    [Column("active")]
    public bool EventActive { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    // Navigation properties
    public IEnumerable<Participant> Participants { get; set; }

    public override string? ToString()
    {
        return base.ToString();
    }
}
