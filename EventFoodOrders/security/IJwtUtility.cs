//2025-03 RR

namespace EventFoodOrders.security;

// JwtUtility manages tokens(Users) in this application, it is completely seperated from Microsofts authentication.
public interface IJwtUtility
{
    string GenerateJwt(string userId, string email);
}