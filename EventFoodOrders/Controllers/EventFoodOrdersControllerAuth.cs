using EventFoodOrders.Api;
using EventFoodOrders.Dto;
using EventFoodOrders.Exceptions;
using EventFoodOrders.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventFoodOrders.Controllers;

[ApiController]
public class EventFoodOrdersControllerAuth(ILogger<EventFoodOrdersControllerAuth> logger, IEventFoodOrdersApi api) : ControllerBase
{
    private readonly ILogger<EventFoodOrdersControllerAuth> _logger = logger;
    private readonly EventFoodOrdersApi _api = (EventFoodOrdersApi)api;

    [HttpPost]
    [Route("/auth/signup")]
    public IActionResult signup(User _user)
    {
        User user;
        try
        {
            user = _api.signup(_user);
        }
        catch (EmailAlreadyExistsException)
        {
            return BadRequest("User with email: " + _user.Email + " already exists");
        }
        catch (Exception)
        {
            return StatusCode(500);
        }

        CookieOptions co = new CookieOptions();
        co.HttpOnly = true;
        co.Expires = DateTime.UtcNow.AddYears(2);
        co.Secure = true;
        co.Path = "/";
        co.SameSite = SameSiteMode.None;

        Response.Cookies.Append("userId", user.id.ToString(), co);

        return Created(string.Empty, user);
    }

    // [HttpPost]
    // [Route("/auth/login")]
    // public IActionResult Login(UserLoginDTO userLoginDTO)
    // {
    //     try
    //     {
    //         LoginResponse loginResponse = _api.Login(userLoginDTO);
    //
    //         return Ok(loginResponse);
    //     }
    //     catch (AccessDeniedException e)
    //     {
    //         _logger.LogWarning("Only admins are allowed to login for " + userLoginDTO.email, e);
    //         return Unauthorized("Only admins are allowed to login");
    //     }
    //     catch (UserNotFoundException e)
    //     {
    //         _logger.LogInformation("No user found with email " + userLoginDTO.email, e);
    //         return NotFound("No user found with email " + userLoginDTO.email);
    //     }
    //     catch (Exception e)
    //     {
    //         _logger.LogError("Failed to login for email " + userLoginDTO.email, e);
    //         return StatusCode(500, "An unexpected error occurred.");
    //     }
    // }
}
