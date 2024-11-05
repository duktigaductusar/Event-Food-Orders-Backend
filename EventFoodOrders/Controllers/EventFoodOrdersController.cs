using EventFoodOrders.Api;
using EventFoodOrders.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventFoodOrders.Controllers;

[ApiController]
public class EventFoodOrdersController(ILogger<EventFoodOrdersController> logger, IEventFoodOrdersApi api) : ControllerBase
{
    private readonly ILogger<EventFoodOrdersController> _logger = logger;
    private readonly EventFoodOrdersApi _api = (EventFoodOrdersApi)api;

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
