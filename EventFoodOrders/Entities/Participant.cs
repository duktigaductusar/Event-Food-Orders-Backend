using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventFoodOrders.Models;

[Table("participants")]
public class Participant
{
    public Participant()
    {
        if (Id == Guid.Empty)
        {
            Id = Guid.NewGuid();
        }
    }

    [Key]
    [Column("participant_id")]
    [Required]
    public Guid Id { get; set; }

    [Column("user_id")]
    [Required]
    public Guid UserId { get; set; }

    [Column("response")]
    [Required]
    public string Response { get; set; }

    [Column("wants_meal")]
    public bool WantsMeal { get; set; }

    [Column("allergies")]
    public string[]? Allergies { get; set; }

    [Column("preferences")]
    public string[]? Preferences { get; set; }

    [ForeignKey("event_id")]
    [Required]
    public Guid EventId { get; set; }

    [Required]
    public Event Event { get; set; }
}
