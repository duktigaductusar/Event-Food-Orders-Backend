using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventFoodOrders.Utilities;

namespace EventFoodOrders.Entities;

[Table("participants")]
public class Participant
{
    public Participant()
    {
        Id = Guid.NewGuid();
        Name = "NAME_MISSING";
        ResponseType = ReType.Pending;
        WantsMeal = false;
        Allergies = "";
        Preferences = "";
    }

    public Participant(Guid userId, Guid eventId)
        : this()
    {
        UserId = userId;
        EventId = eventId;
    }

    [Key]
    [Column("participant_id")]
    [Required]
    public Guid Id { get; init; }

    [Column("user_id")]
    [Required]
    public Guid UserId { get; init; }

    [ForeignKey("event_id")]
    [Column("event_id")]
    [Required]
    public Guid EventId { get; set; }

    [Column("name")]
    [Required]
    public string Name { get; set; }

    [Column("response_type")]
    [Required]
    public string ResponseType { get; set; }

    [Column("wants_meal")]
    public bool WantsMeal { get; set; }

    [Column("allergies")]
    public string? Allergies { get; set; }

    [Column("preferences")]
    public string? Preferences { get; set; }
}
