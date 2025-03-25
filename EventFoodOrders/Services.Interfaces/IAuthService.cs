using EventFoodOrders.security;

namespace EventFoodOrders.Interfaces;

public interface IAuthService
{
    string GetLoginUrl();
    Task<AuthResponse> ExchangeCodeForTokenAsync(string code);
}