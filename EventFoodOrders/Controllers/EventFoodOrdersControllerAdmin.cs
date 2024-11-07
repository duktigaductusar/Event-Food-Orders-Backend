

using EventFoodOrders.Api;
using EventFoodOrders.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventFoodOrders.Controllers;

[ApiController]
public class EventFoodOrdersControllerAdmin(ILogger<EventFoodOrdersControllerAdmin> logger, IEventFoodOrdersApi api) : ControllerBase
{
    private readonly ILogger<EventFoodOrdersControllerAdmin> _logger = logger;
    private readonly EventFoodOrdersApi _api = (EventFoodOrdersApi)api;

    // Admin Start

    // TODO Post /admin/users
    [HttpPost]
    [Route("/admin/users")]
    public IActionResult AddUsers(User _user)
    {
        User user = _api.AddUser(_user);

        return Ok(user);
    }

    // TODO Put /admin/users/{id}
    [HttpPut]
    [Route("/admin/users/{id}")]
    public IActionResult UpdateUsers(String id, [FromBody] User _user)
    {
        Guid GuidId = new Guid(id);

        User existingUser = _api.GetUser(GuidId);

        if (existingUser == null)
        {
            // TODO return 400
        }

        // TODO Map values from _user to existing user

        User user = _api.UpdateUser(existingUser);

        return Ok(user);
    }

    // TODO Get /admin/users
    [HttpGet]
    [Route("/admin/users")]
    public IActionResult GetUsers()
    {
        List<User> users = _api.GetUsers();

        return Ok(users);
    }

    // TODO Get /admin/search ? String name
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
    public IActionResult CreateEvent(Event _event)
    {
        Event retVal = _api.CreateEvent(_event);

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

    // TODO Get /admin/events/{id}
    [HttpGet]
    [Route("/admin/events/{id}")]
    public IActionResult GetEvent(string id)
    {
        Event existingEvent = _api.GetEvent(new Guid(id));

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


        return Ok(participants);
    }

    // Admin End



    //[HttpGet]
    //[Route("/api/participants")]
    //public IActionResult GetParticipants()
    //{
    //    List<Participant> participants = _api.GetParticipants().ToList<Participant>();

    //    return Ok(participants);
    //}


    //[HttpPost]
    //[Route("/api/add/participants")]
    //public IActionResult newParticipant(Participant participant)
    //{
    //    return Ok(_api.CreateParticipant(participant));
    //}

    //[HttpPost]
    //[Route("/api/add/user")]
    //public IActionResult newUser(User user)
    //{
    //    return Ok(_api.AddUser(user));
    //}

}
