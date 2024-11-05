using EventFoodOrders.security;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventFoodOrders.Models;

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
        Authority = Role.PARTICIPANT.ToString();
    }

    [Key]
    [Column("user_id")]
    private Guid user_id { get; }

    [Column]
    private String name { get; set; }

    //@Column(unique = true, nullable = false)
    [Column]
    private String email { get; set; }

    [Column]
    private String password { get; set; }

    //@Enumerated(EnumType.STRING)
    [Column]
    private Role role;

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

    //@Override
    public List<IGrantedAuthority> getAuthorities()
    {
        List<IGrantedAuthority> retVal = new List<IGrantedAuthority>();

        return retVal;
    }
}

