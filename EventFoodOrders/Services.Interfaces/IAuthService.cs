using EventFoodOrders.security;

namespace EventFoodOrders.Services.Interfaces;

public interface IAuthService
{
    string GetLoginUrl();
    Task<AuthResponse> ExchangeCodeForTokenAsync(string code);
}