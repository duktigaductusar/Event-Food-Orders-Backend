

using EventFoodOrders.Api;
using EventFoodOrders.Dto;
using EventFoodOrders.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace EventFoodOrders.Controllers;

[ApiController]
public class EventFoodOrdersControllerAuth(ILogger<EventFoodOrdersControllerAuth> logger, IEventFoodOrdersApi api) : ControllerBase
{
    private readonly ILogger<EventFoodOrdersControllerAuth> _logger = logger;
    private readonly EventFoodOrdersApi _api = (EventFoodOrdersApi)api;

    // Auth Start
    // TODO Post /signup
    [HttpPost]
    [Route("/auth/signup")]
    public Object signup(Guid id)
    {
        throw new NotImplementedException();
        //        return Ok();
    }

    // TODO Post /login
    [HttpPost]
    [Route("/auth/login")]
    public IActionResult Login(UserLoginDTO userLoginDTO)
    {
        try
        {
            LoginResponse loginResponse = _api.Login(userLoginDTO);

            return Ok(loginResponse);
        }
        catch (AccessDeniedException e)
        {
            // TODO Fix to add exception e to log
            _logger.LogWarning("Only admins are allowed to login for " + userLoginDTO.email);
            return Unauthorized("Only admins are allowed to login");
        }
        catch (UserNotFoundException e)
        {
            _logger.LogInformation("No user found with email " + userLoginDTO.email, e);
            return NotFound("No user found with email " + userLoginDTO.email);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to login for email " + userLoginDTO.email, e);
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}
