using EventFoodOrders.security;

namespace EventFoodOrders.Interfaces;

public interface IAuthorizationService
{
    string GetLoginUrl();
    Task<AuthResponse> ExchangeCodeForTokenAsync(string code);
}