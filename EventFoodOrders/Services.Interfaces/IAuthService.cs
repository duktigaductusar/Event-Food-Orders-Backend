using EventFoodOrders.security;

namespace EventFoodOrders.Services.Interfaces;

/// <summary>
/// Handles authentication for backend.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Returns the user name/display name of a user using their id from Entra ID.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<string> GetUserName(Guid userId);

    /// <summary>
    /// Returns the proper URL for logging in to Entra ID.
    /// </summary>
    /// <returns></returns>
    string GetLoginUrl();

    /// <summary>
    /// Exchanges a login token for an access token.
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    // Task<AuthResponse> ExchangeCodeForTokenAsync(string code);
}