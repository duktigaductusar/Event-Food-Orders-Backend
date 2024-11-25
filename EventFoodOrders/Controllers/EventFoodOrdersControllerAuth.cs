

using EventFoodOrders.Api;
using EventFoodOrders.Dto;
using EventFoodOrders.Models;
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
    public LoginResponse Login(UserLoginDTO userLoginDTO)
    {
        throw new NotImplementedException();
        //        return Ok();
    }
    // TODO Post /change-password  --- Check Java impl for url
    [HttpPost]
    [Route("/auth/change-password")]
    public LoginResponse changePassword(UserLoginDTO userLoginDTO)
    {
        throw new NotImplementedException();
        //        return Ok();
    }
    // Auth End



}
