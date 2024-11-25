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
    private Guid user_id;

    private Guid id
    { get { return user_id; } }


    [Column("name")]
    [Required]
    [DataType(DataType.Text)]
    private string Name;

    [Column("email")]
    private String Email { get; set; }

    [Column("allergies")]
    private String allergies;

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

    public Guid getId()
    {
        return id;
    }

    public String getName()
    {
        return Name;
    }

    public void setName(String name)
    {
        this.Name = name;
    }

    public String getEmail()
    {
        return Email;
    }

    public void setEmail(String _email)
    {
        this.Email = _email;
    }

    public String getAllergies()
    {
        return allergies;
    }

    public void setAllergies(String _allergies)
    {
        this.allergies = _allergies;
    }

    public Role getRole()
    {
        return (Role)Enum.Parse(typeof(Role), Role);
    }

    public void setRole(Role _role)
    {
        this.Role = _role.ToString();
    }

}

