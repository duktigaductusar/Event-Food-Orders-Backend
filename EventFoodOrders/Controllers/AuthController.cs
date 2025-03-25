using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using EventFoodOrders.security;
using EventFoodOrders.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace EventFoodOrders.Controllers;

[ApiController]
[Route("api/")]
public class AuthController(IServiceManager serviceManager, IJwtUtility jwtUtility, IWebHostEnvironment env) : ControllerBase
{
    private readonly IAuthService _authService = serviceManager.AuthService;
    private readonly IJwtUtility _jwtUtility;
    private readonly IWebHostEnvironment _env;

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
        Guid userGuid = Guid.Parse(userId);
        if (userGuid == Guid.Empty || userGuid == null)
        {
            return BadRequest("Parsing to Guid failed or returned null.");
        }
        
        string userEmail = authResponse.Email;
        
        var jwt = _jwtUtility.GenerateJwt(userId, userEmail);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = !_env.IsDevelopment(),
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