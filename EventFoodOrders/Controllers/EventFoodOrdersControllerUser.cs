

using EventFoodOrders.Api;
using EventFoodOrders.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventFoodOrders.Controllers;

[ApiController]
public class EventFoodOrdersControllerUser(ILogger<EventFoodOrdersControllerUser> logger, IEventFoodOrdersApi api) : ControllerBase
{
    private readonly ILogger<EventFoodOrdersControllerUser> _logger = logger;
    private readonly EventFoodOrdersApi _api = (EventFoodOrdersApi)api;



    // Keep
    // TODO Put /admin/users/{id}
    [HttpPut]
    [Route("/admin/users/{id}")]
    public IActionResult UpdateUsers(String id, [FromBody] User _user)
    {
        User existingUser = _api.GetUser(new Guid(id));

        if (existingUser == null)
        {
            return BadRequest();
        }

        // TODO Map values from _user to existing user

        User user = _api.UpdateUser(existingUser);

        return Ok(user);
    }

    // Keep
    // TODO Get /admin/users
    [HttpGet]
    [Route("/admin/users")]
    public IActionResult GetUsers()
    {
        List<User> users = _api.GetUsers();

        return Ok(users);
    }

    // Keep
    // TODO Get /admin/events
    [HttpGet]
    [Route("/admin/events")]
    public IActionResult GetEvents()
    {
        List<Event> events = _api.GetEvents();

        return Ok(events);
    }


    // TODO Get /admin/events/{id}
    [HttpGet]
    [Route("/admin/events/{id}")]
    public IActionResult getEventById(string id)
    {
        Event retVal = _api.GetEvent(new Guid(id));

        return Ok(retVal);
    }



}
