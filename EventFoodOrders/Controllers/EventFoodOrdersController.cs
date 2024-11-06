

using EventFoodOrders.Api;
using EventFoodOrders.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventFoodOrders.Controllers;

[ApiController]
public class EventFoodOrdersController(ILogger<EventFoodOrdersController> logger, IEventFoodOrdersApi api) : ControllerBase
{
    private readonly ILogger<EventFoodOrdersController> _logger = logger;
    private readonly EventFoodOrdersApi _api = (EventFoodOrdersApi)api;

    /**
     * I java implementationen är det 3 Controllers.
     * * Admin
     * * Auth
     * * Participant
     * 
     * "Målet" är att vi har samma RESTApi i både Java impl och C# impl
     */


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
    // TODO Post /admin/events 
    // TODO Put /admin/events/{id}
    // TODO Get /admin/events/{id}
    // TODO Delete /admin/events/{id}
    // TODO Get /admin/events/{eventId}/participants-with-meal

    // Admin End


    // Auth Start
    // TODO Post /signup
    // TODO Post /login
    // TODO Post /change-password  --- Check Java impl for url
    // Auth End


    // Participant Start
    //TODO Post /participants/register
    //TODO Put /participants/update/{id}
    //TODO Get /participants/events
    //TODO Delete /participants/cancel/{id}

    // Participant End


















    [HttpGet]
    [Route("/api/participants")]
    public IActionResult GetParticipants()
    {
        List<Participant> participants = _api.GetParticipants().ToList<Participant>();

        return Ok(participants);
    }


    [HttpPost]
    [Route("/api/add/participants")]
    public IActionResult newParticipant(Participant participant)
    {
        return Ok(_api.CreateParticipant(participant));
    }

    [HttpPost]
    [Route("/api/add/user")]
    public IActionResult newUser(User user)
    {
        return Ok(_api.AddUser(user));
    }

}
