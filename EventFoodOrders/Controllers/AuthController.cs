using EventFoodOrders.Repositories;
using EventFoodOrders.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EventFoodOrders.Services.Interfaces;

namespace EventFoodOrders.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IServiceManager serviceManager, IGraphRepository graphRepository) : ControllerBase
{
    private readonly IAuthService _authService = serviceManager.AuthService;
    private readonly IGraphRepository _graphRepository = graphRepository;

    [Authorize]
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

    [HttpGet("graph")]
    public async Task<IActionResult> Graph(Guid userId)
    {
        try
        {
            var user = await _graphRepository.GetUserAsync(userId);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error when fetching user from Graph API: {ex.Message}");
        }
    }

    [HttpPost("mail")]
    public async Task<IActionResult> Mail(Guid[] userIds)
    {
        try
        {
            await _graphRepository.SendMailAsync(userIds);
            return Ok("Emails sent");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error when sending mail: {ex.Message}");
        }
    }

    
    [HttpGet("/login")]
    public IActionResult Login()
    {
        string loginUrl = _authService.GetLoginUrl();
        return Redirect(loginUrl);
    }
}