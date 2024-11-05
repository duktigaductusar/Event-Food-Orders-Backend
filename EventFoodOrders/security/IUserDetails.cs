namespace EventFoodOrders.security;

/**
* Provides core user information.
*
* <p>
* Implementations are not used directly by Spring Security for security purposes. They
* simply store user information which is later encapsulated into {@link Authentication}
* objects. This allows non-security related user information (such as email addresses,
* telephone numbers etc) to be stored in a convenient location.
* <p>
* Concrete implementations must take particular care to ensure the non-null contract
* detailed for each method is enforced. See
* {@link org.springframework.security.core.userdetails.User} for a reference
* implementation (which you might like to extend or use in your code).
*
* @author Ben Alex
* @see UserDetailsService
* @see UserCache
*/
public interface IUserDetails : IGrantedAuthority
{

    /**
     * Returns the authorities granted to the user. Cannot return <code>null</code>.
     * @return the authorities, sorted by natural key (never <code>null</code>)
     */
    List<IGrantedAuthority> getAuthorities();

    /**
     * Returns the password used to authenticate the user.
     * @return the password
     */
    string getPassword();

    /**
     * Returns the username used to authenticate the user. Cannot return
     * <code>null</code>.
     * @return the username (never <code>null</code>)
     */
    string getUsername();

    /**
     * Indicates whether the user's account has expired. An expired account cannot be
     * authenticated.
     * @return <code>true</code> if the user's account is valid (ie non-expired),
     * <code>false</code> if no longer valid (ie expired)
     */
    bool isAccountNonExpired();

    /**
     * Indicates whether the user is locked or unlocked. A locked user cannot be
     * authenticated.
     * @return <code>true</code> if the user is not locked, <code>false</code> otherwise
     */
    bool isAccountNonLocked();

    /**
     * Indicates whether the user's credentials (password) has expired. Expired
     * credentials prevent authentication.
     * @return <code>true</code> if the user's credentials are valid (ie non-expired),
     * <code>false</code> if no longer valid (ie expired)
     */
    bool isCredentialsNonExpired();

    /**
     * Indicates whether the user is enabled or disabled. A disabled user cannot be
     * authenticated.
     * @return <code>true</code> if the user is enabled, <code>false</code> otherwise
     */
    bool isEnabled();

}
