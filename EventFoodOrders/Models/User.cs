using EventFoodOrders.security;
using System.ComponentModel.DataAnnotations;

namespace EventFoodOrders.Models;

public class User /* : IUserDetails */
{
    public User()
    {
        if (user_id == Guid.Empty)
        {
            user_id = Guid.NewGuid();
        }
    }

    [Key]
    private Guid user_id { get; }

    private String name { get; set; }

    //@Column(unique = true, nullable = false)
    private String email { get; set; }

    private String password { get; set; }

    //@Enumerated(EnumType.STRING)
    private Role role { get; set; }

    //@Override
    public List<IGrantedAuthority> getAuthorities()
    {
        //return IList < Role > = [];
        return null;
    }

    public string getAuthority()
    {
        return role.ToString();
    }

    public bool isAccountNonExpired()
    {
        return false;
    }

    public bool isAccountNonLocked()
    {
        return false;
    }

    public bool isCredentialsNonExpired()
    {
        return false;
    }

    public bool isEnabled()
    {
        return false;
    }
}

