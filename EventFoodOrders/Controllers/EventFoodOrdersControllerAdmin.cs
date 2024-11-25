

using EventFoodOrders.Api;
using EventFoodOrders.Dto;
using EventFoodOrders.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventFoodOrders.Controllers;

[ApiController]
public class EventFoodOrdersControllerAdmin(ILogger<EventFoodOrdersControllerAdmin> logger, IEventFoodOrdersApi api) : ControllerBase
{
    private readonly ILogger<EventFoodOrdersControllerAdmin> _logger = logger;
    private readonly EventFoodOrdersApi _api = (EventFoodOrdersApi)api;




    // TODO Post /admin/users
    [HttpPost]
    [Route("/admin/users")]
    public IActionResult AddUsers(UserDTO _user)
    {
        if (_user == null)
        {
            return BadRequest();
        }

        User user = new();
        user.setName(_user.getName());
        user.setEmail(_user.getEmail());


        // TODO fix mapping or let API fix it.



        //   User user = _api.AddUser(_user);

        return Ok(user);
    }


    // TODO Get /admin/search ? String name. Inte säker på att det behövs. Filtrering/Sök går att göra i frontend.
    [HttpGet]
    [Route("/admin/search")]
    public IActionResult searchUsersByName()
    {
        String Name = "";

        //String Name =  Request.QueryString["fullname"]; FÅR INTE ATT FUNGERA

        List<User> users = _api.FindByName(Name);

        return Ok(users);
    }


    // TODO Delete /admin/users/{id}
    [HttpDelete]
    [Route("/admin/users/{id}")]
    public IActionResult DeleteUsers(String id)
    {
        _api.DeleteUser(new Guid(id));

        return Ok();
    }

    // TODO Post /admin/events 
    [HttpPost]
    [Route("/admin/events")]
    public IActionResult AddEvent(EventDTO _event)
    {
        Event retVal = _api.SaveEvent(_event);

        return Ok(retVal);
    }

    // TODO Put /admin/events/{id}
    [HttpPut]
    [Route("/admin/events/{id}")]
    public IActionResult UpdateEvent(string id, Event _event)
    {
        Event existingEvent = _api.GetEvent(new Guid(id));

        //TODO Map _event -> existingEvent

        Event retVal = _api.UpdateEvent(existingEvent);

        return Ok(retVal);
    }

    // TODO Delete /admin/events/{id}
    [HttpDelete]
    [Route("/admin/events/{id}")]
    public IActionResult DeleteEvent(string id)
    {
        _api.DeleteEvent(new Guid(id));

        return Ok();
    }
    // TODO Get /admin/events/{eventId}/participants-with-meal
    [HttpGet]
    [Route("/admin/events/{eventId}/participants-with-meal")]
    public IActionResult getEventWithMeal(string eventId)
    {
        List<Participant> participants = _api.getEventWithMeal(new Guid(eventId));
        if (participants == null || participants.Count > 0)
        {
            return NotFound();
        }

        return Ok(participants);
    }

}
