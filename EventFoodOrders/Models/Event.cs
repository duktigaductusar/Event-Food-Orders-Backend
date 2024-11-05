//@Data
//@Entity
//@Table(name = "events")
using System.ComponentModel.DataAnnotations;

public class Event
{
    public Event()
    {
        if (event_id == Guid.Empty)
        {
            event_id = Guid.NewGuid();
        }
    }

    [Key]
    private Guid event_id { get; }


    //@Column(nullable = false)
    private DateTime eventDate { get; set; }

    //@Column(nullable = false)
    private bool active = true;

}
