using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using IAuthorizationService = EventFoodOrders.Interfaces.IAuthorizationService;

namespace EventFoodOrders.Controllers;

[ApiController]
[Route("api/")]
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
        string loginUrl = _authService.GetLoginUrl();
        return Redirect(loginUrl);
    }

    [HttpGet("[controller]/callback")]
    public async Task<IActionResult> Callback([FromQuery] string code)
    {
        var authResponse = await _authService.ExchangeCodeForTokenAsync(code);
        return Ok(authResponse);
    }

    [HttpGet("[controller]/logout")]
    public IActionResult Logout()
    {
        return SignOut(new AuthenticationProperties { RedirectUri = "/login" },
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme);
    }

    [HttpGet("[controller]/status")]
    public IActionResult Status()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return Ok(new
            {
                IsAuthenticated = true,
                Name = User.Identity.Name,
                Email = User.Claims.FirstOrDefault(c => c.Type == "email")?.Value
            });
        }
        return Unauthorized(new {IsAuthenticated = false});
    }
}