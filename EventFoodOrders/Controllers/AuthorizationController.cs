using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

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
        throw new NotImplementedException();
    }

    [HttpGet("[controller]/callback")]
    public async Task<IActionResult> Callback([FromQuery] string code)
    {
        throw new NotImplementedException();
    }

    [HttpGet("[controller]/logout")]
    public async Task<IActionResult> Logout()
    {
        throw new NotImplementedException();
    }
}