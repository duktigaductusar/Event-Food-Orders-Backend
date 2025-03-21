using System.Collections.Immutable;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EventFoodOrders.security;
using Microsoft.AspNetCore.Authorization;
using IAuthorizationService = EventFoodOrders.Interfaces.IAuthorizationService;

namespace EventFoodOrders.Controllers;

[ApiController]
[Route("api/")]
public class AuthorizationController : ControllerBase
{
    private readonly IAuthorizationService _authService;
    private readonly IJwtUtility _jwtUtility;
    private readonly IWebHostEnvironment _env;

    public AuthorizationController(IAuthorizationService authService, IJwtUtility jwtUtility, IWebHostEnvironment env)
    {
        _authService = authService;
        _jwtUtility = jwtUtility;
        _env = env;
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
        if (string.IsNullOrEmpty(code))
        {
            return BadRequest("Authorization code is missing.");
        }
        var authResponse = await _authService.ExchangeCodeForTokenAsync(code);
        string userId = authResponse.UserId;
        string userEmail = authResponse.Email;
        
        var jwt = _jwtUtility.GenerateJwt(userId, userEmail);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = !_env.IsDevelopment(),
            //Prod setting
            // SameSite = SameSiteMode.Strict,
            //Dev setting
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddHours(1)
        };
        
        //Cookie with the JWT is appended to the redirect, frontend now has necessary information stored securely.
        Response.Cookies.Append("jwt_token", jwt, cookieOptions);
        return Redirect("http://localhost:4200/create-event");
    }

    [HttpGet("[controller]/logout")]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete("jwt_token");
        HttpContext.Session?.Clear();
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        var logoutUrl = $"https:login.microsoftonline.com/{Environment.GetEnvironmentVariable("AzureAd__TenantId")}/oauth2/v2.0/logout?post_logout_redirect_uri=localhost:4200/login";
        return Redirect(logoutUrl);
    }

    [Authorize(AuthenticationSchemes = "Jwt")]
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