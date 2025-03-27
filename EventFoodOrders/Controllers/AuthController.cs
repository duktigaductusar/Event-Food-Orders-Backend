using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
}