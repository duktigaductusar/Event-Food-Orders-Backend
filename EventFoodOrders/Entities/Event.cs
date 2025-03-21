using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventFoodOrders.Entities;

[Table("events")]
public class Event
{
    public Event()
    {
        
    }

    public Event(Guid userId)
    {
        Id = Guid.NewGuid();
        OwnerId = userId;
        Title = "";
        Description = "";
        Date = DateTime.UtcNow;
        Deadline = DateTime.UtcNow;
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

    // Foreign key
    //[Required]
    [ForeignKey("owner_id")]
    [Column("owner_id")]
    public Guid OwnerId { get; set; }

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

    public override string ToString()
    {
        return Title;
    }
}
