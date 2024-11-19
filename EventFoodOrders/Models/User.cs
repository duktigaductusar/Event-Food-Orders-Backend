using EventFoodOrders.security;
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

        IsEnabled = true;
        IsCredentialsNonExpired = false;
        IsAccountNonLocked = false;
        IsAccountNonExpired = false;
        Authority = Models.Role.PARTICIPANT.ToString();
    }

    [Key]
    [Required]
    [Column("user_id")]
    private Guid user_id { get; }

    [Column("name")]
    [Required]
    [DataType(DataType.Text)]
    private string Name { get; set; }

    [Column("email")]
    private String Email { get; set; }

    [Column]
    private String Password { get; set; }

    [Column]
    private string Role { get; set; }

    [Column]
    private bool IsEnabled { get; set; }

    [Column]
    private bool IsCredentialsNonExpired { get; set; }

    [Column]
    private bool IsAccountNonLocked { get; set; }

    [Column]
    private bool IsAccountNonExpired { get; set; }

    [Column]
    private string Authority { get; set; }

    public List<IGrantedAuthority> getAuthorities()
    {
        List<IGrantedAuthority> retVal = new List<IGrantedAuthority>();

        return retVal;
    }
}

