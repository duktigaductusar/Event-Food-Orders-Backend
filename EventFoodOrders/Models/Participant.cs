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


    //@Id
    //@GeneratedValue(strategy = GenerationType.UUID)
    //@Column(name = "participant_id", updatable = false, nullable = false)
    //[DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    [Column("participant_id")]
    private Guid participant_id;

    //@ManyToOne
    //@JoinColumn(name = "user_id", nullable = false)
    [Column("user_id")]
    private User user;

    //@ManyToOne
    //@JoinColumn(name = "event_id", nullable = false)
    //[Column("event_id")]
    //private Event event;

    private bool wantsMeal;

    private String allergies;

}
