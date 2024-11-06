//@Data
//@Entity
//@Table(name = "events")
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("events")]
public class Event
{
    public Event()
    {
        if (event_id == Guid.Empty)
        {
            event_id = Guid.NewGuid();
        }
        active = false;
    }

    [Key]
    [Column("event_id")]
    [Required]
    private Guid event_id { get; }


    [Required]
    [Column("event_date")]
    private DateTime eventDate { get; set; }

    [Required]
    [Column("active")]
    private bool active { get; set; }
}
