using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventFoodOrders.Utilities;

namespace EventFoodOrders.Entities;

[Table("participants")]
public class Participant
{
    public Participant()
    {
        if (Id == Guid.Empty)
        {
            Id = Guid.NewGuid();
        }
        if (Name == string.Empty || Name is null)
        {
            Name = "";
        }
        if (Response == string.Empty || Response is null)
        {
            Response = ReType.Pending;
        }
    }

    [Key]
    [Column("participant_id")]
    [Required]
    public Guid Id { get; set; }

    [Column("user_id")]
    [Required]
    public Guid UserId { get; set; }

    [ForeignKey("event_id")]
    [Required]
    public Guid EventId { get; set; }

    [Column("name")]
    [Required]
    public string Name { get; set; }

    [Column("response")]
    [Required]
    public string Response { get; set; }

    [Column("wants_meal")]
    public bool WantsMeal { get; set; }

    [Column("allergies")]
    public string[]? Allergies { get; set; }

    [Column("preferences")]
    public string[]? Preferences { get; set; }

    [Required]
    public Event Event { get; set; }
}
