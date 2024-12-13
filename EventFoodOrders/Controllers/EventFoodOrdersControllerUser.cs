

using EventFoodOrders.Api;
using EventFoodOrders.Exceptions;
using EventFoodOrders.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventFoodOrders.Controllers;

[ApiController]
public class EventFoodOrdersControllerUser(ILogger<EventFoodOrdersControllerUser> logger, IEventFoodOrdersApi api) : ControllerBase
{
    private readonly ILogger<EventFoodOrdersControllerUser> _logger = logger;
    private readonly EventFoodOrdersApi _api = (EventFoodOrdersApi)api;



    // Keep
    // TODO Put /user/users/{id}
    [HttpPut]
    [Route("/user/users/{id}")]
    public IActionResult UpdateUsers(String id, [FromBody] User _user)
    {
        User? user = null;
        try
        {
            user = _api.UpdateUser(id, _user);
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UserNotFoundException ex)
        {
            return StatusCode(404, ex);
        }
        catch
        {
            return StatusCode(500);
        }
        if (user == null)
        {
            return StatusCode(404);

        }

        return Ok(user);
    }

    [HttpGet]
    [Route("/user/users")]
    public IActionResult GetUsers()
    {
        List<User> users = _api.GetUsers();

        return Ok(users);
    }

    [HttpGet]
    [Route("/user/events")]
    public IActionResult GetEvents()
    {
        List<Event> events = _api.GetEvents();

        return Ok(events);
    }

    [HttpGet]
    [Route("/user/events/{id}")]
    public IActionResult getEventById(string id)
    {
        Event retVal = _api.GetEvent(new Guid(id));

        return Ok(retVal);
    }

    // TODO new method, generateRegistrationLink. Check java impl
    /*    @GetMapping("/registration-link/{id}")
    public ResponseEntity<String> generateRegistrationLink(@PathVariable UUID id)
        {
            String registrationLink = REGISTRATION_BASE_URL + "?id=" + id;
            return ResponseEntity.ok(registrationLink);
        }
    */

    [HttpGet]
    [Route("/user/{eventId}/registrations-count")]
    public IActionResult getRegistrationsCount(String eventId)
    {
        try
        {
            long count = _api.getRegistrationsCount(new Guid(eventId));
            if (count == 0) { return NotFound(); }

            return Ok(count);
        }
        catch (Exception ex) { return BadRequest(ex); }
    }


}
