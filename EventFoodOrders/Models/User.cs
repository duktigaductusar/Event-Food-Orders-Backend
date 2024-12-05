using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventFoodOrders.Models;
[Table("users")]
public class User
{
    public User()
    {
        if (user_id == Guid.Empty)
        {
            user_id = Guid.NewGuid();
        }
    }

    [Key]
    [Required]
    [Column("user_id")]
    public Guid user_id { get; set; }

    [Column("name")]
    [Required]
    [DataType(DataType.Text)]
    public string Name { get; set; }

    [Column("email")]
    public String Email { get; set; }

    [Column("allergies")]
    public String allergies { get; set; }

    [Column]
    public string Role { get; set; }
}

