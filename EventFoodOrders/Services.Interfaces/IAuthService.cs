using EventFoodOrders.security;

namespace EventFoodOrders.Services.Interfaces;

/// <summary>
/// Handles authentication for backend.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Returns the proper URL for logging in to Entra ID.
    /// </summary>
    /// <returns></returns>
    // string GetLoginUrl();

    /// <summary>
    /// Exchanges a login token for an access token.
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    // Task<AuthResponse> ExchangeCodeForTokenAsync(string code);
}