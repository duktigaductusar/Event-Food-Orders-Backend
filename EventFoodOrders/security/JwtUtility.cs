//2025-03 RR

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EventFoodOrders.security;

public class JwtUtility(IConfiguration config) : IJwtUtility
{
    private readonly IConfiguration _config = config;

    public string GenerateJwt(string userId, string email)
    {
        // 1. Claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Email, email),
        };
        
        // 2. Sign-in credentials
        var secret = _config["JWT:Secret"];
        var issuer = _config["JWT:Issuer"];
        var audience = _config["JWT:Audience"];
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        // 3. Token
        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );
        
        // 4. JWT string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}