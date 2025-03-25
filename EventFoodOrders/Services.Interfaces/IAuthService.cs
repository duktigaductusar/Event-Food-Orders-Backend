using EventFoodOrders.security;

namespace EventFoodOrders.Services.Interfaces;

// Handles authentication for backend
public interface IAuthService
{
    // Returns the proper URL for logging in to Entra ID.
    string GetLoginUrl();

    // Exchanges a login token for an access token.
    Task<AuthResponse> ExchangeCodeForTokenAsync(string code);
}