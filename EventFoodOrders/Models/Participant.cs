using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventFoodOrders.Models;

[Table("participants")]
public class Participant
{
    public Participant()
    {
        if (participant_id == Guid.Empty)
        {
            participant_id = Guid.NewGuid();
        }
        allergies = "";
    }

    [Key]
    [Column("participant_id")]
    [Required]
    public Guid participant_id { get; set; }

    //@ManyToOne
    //@JoinColumn(name = "user_id", nullable = false)
    [Column("user_id")]
    [Required]
    public Guid _user { get; set; }

    //@ManyToOne
    //@JoinColumn(name = "event_id", nullable = false)
    [Column("event_id")]
    [Required]
    public Guid _event { get; set; }

    [Column("wants_meal")]
    public bool wantsMeal { get; set; }

    [Column("allergies")]
    public String allergies { get; set; }

}
