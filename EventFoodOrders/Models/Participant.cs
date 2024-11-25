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
    }


    [Key]
    [Column("participant_id")]
    [Required]
    private Guid participant_id;

    public Guid id
    { get { return participant_id; } }

    //@ManyToOne
    //@JoinColumn(name = "user_id", nullable = false)
    [Column("user_id")]
    [Required]
    private User user;

    //@ManyToOne
    //@JoinColumn(name = "event_id", nullable = false)
    [Column("event_id")]
    [Required]
    private Event _event;

    public bool wantsMeal;

    private String allergies;

}
