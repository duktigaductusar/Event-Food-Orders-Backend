using EventFoodOrders.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using EventFoodOrders.Services.Interfaces;

namespace EventFoodOrders.Controllers;

[ApiController]
[Route("api/")]
public class AuthController(IServiceManager serviceManager) : ControllerBase
{
    private readonly IAuthService _authService = serviceManager.AuthService;

    [Authorize]
    [HttpGet("[controller]/status")]
    public IActionResult Status()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            var res =  (new
            {
                IsAuthenticated = true,
                Name = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value,
                Email = User.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value,
                UserId = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value,
                
            });
            return Ok(res);
        }
        return Unauthorized(new {IsAuthenticated = false});
    }

    private bool ValidateAzureAdToken(string token, out JwtSecurityToken jwtToken)
    {
        jwtToken = null;
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            jwtToken = tokenHandler.ReadJwtToken(token);
            //ToDo - Proper validation towards EntraId
            return true;
        }
        catch
        {
            return false;
        }
    }

    public class TokenExchangeRequest
    {
        public string Token { get; set; }
    }
}