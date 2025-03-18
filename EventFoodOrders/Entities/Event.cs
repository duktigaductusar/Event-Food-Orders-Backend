using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventFoodOrders.Models;

[Table("events")]
public class Event
{
    public Event()
    {
        if (Id == Guid.Empty)
        {
            Id = Guid.NewGuid();
        }
        Title = "";
        Participants = [];
    }

    public Event(Guid guid)
    {
        Id = guid;
        Title = "";
        Participants = [];
    }

    [Key]
    [Column("id")]
    [Required]
    public Guid Id { get; set; }

    [Required]
    [Column("title")]
    public string Title { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Required]
    [Column("date")]
    public DateTimeOffset Date { get; set; }

    [Required]
    [Column("deadline")]
    public DateTimeOffset Deadline { get; set; }

    // Navigation properties
    public Collection<Participant> Participants { get; set; }

    // Methods
    public bool IsActive()
    {
        if (DateTime.UtcNow < Date)
        {
            return true;
        }
        return false;
    }
}
