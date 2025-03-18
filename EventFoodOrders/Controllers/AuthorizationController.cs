using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EventFoodOrders.Interfaces;
using Microsoft.AspNetCore.Authorization;
using IAuthorizationService = EventFoodOrders.Interfaces.IAuthorizationService;

namespace EventFoodOrders.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthorizationController : ControllerBase
{
    private readonly IAuthorizationService _authService;

    public AuthorizationController(IAuthorizationService authService)
    {
        _authService = authService;
    }

    [HttpGet("[controller]/login")]
    public IActionResult Login()
    {
        var loginUrl = _authService.GetLoginUrl();
        return Redirect(loginUrl);
    }

    [HttpGet("[controller]/callback")]
    public async Task<IActionResult> Callback([FromQuery] string code)
    {
        var authResponse = await _authService.ExchangeCodeForTokenAsync(code);
        return Ok(authResponse);
    }

    [HttpGet("[controller]/logout")]
    public async Task<IActionResult> Logout()
    {
        throw new NotImplementedException();
    }
}