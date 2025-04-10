using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EventFoodOrders.Services.Interfaces;

namespace EventFoodOrders.Controllers;

//[Authorize] //Un-comment when ready for full auth flow
[ApiController]
[Route("api/[controller]")]
public class AuthController(IServiceManager serviceManager) : ControllerBase
{
    private readonly IAuthService _authService = serviceManager.AuthService;

    [HttpGet("status")]
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

    //[HttpGet("graph")]
    //public async Task<IActionResult> Graph(Guid userId)
    //{
    //    try
    //    {
    //        var user = await serviceManager.UserService.GetUserWithId(userId);
    //        return Ok(user);
    //    }
    //    catch (Exception ex)
    //    {
    //        return StatusCode(500, $"Internal server error when fetching user from Graph API: {ex.Message}");
    //    }
    //}
}