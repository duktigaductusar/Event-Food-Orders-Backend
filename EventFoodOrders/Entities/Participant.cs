using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

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
        // ToDo: Look over how to handle user data
        //user = new User();
    }

    [Key]
    [Column("participant_id")]
    [Required]
    public Guid participant_id { get; set; }

    //@ManyToOne
    //@JoinColumn(name = "user_id", nullable = false)
    //[Column("user_id")
    //[Required]
    //Guid user_id;


    // ToDo: Look over how to handle user data
    /*
    [ForeignKey("user_id")]
    //[Column("user_id")]
    [Required]
    [NotNull]
    public User user { get; set; }
    */



    //@ManyToOne
    //@JoinColumn(name = "event_id", nullable = false)
    //[Column("event_id")]
    //[Required]
    //public Guid _event { get; set; }

    [Column("wants_meal")]
    public bool wantsMeal { get; set; }

    [Column("allergies")]
    public string allergies { get; set; }

    [ForeignKey("event_id")]
    [Required]
    public Guid EventId { get; set; }

}
