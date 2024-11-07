using EventFoodOrders.Api;
using EventFoodOrders.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventFoodOrders.Controllers;

[ApiController]
public class EventFoodOrdersControllerParticipant(ILogger<EventFoodOrdersControllerParticipant> logger, IEventFoodOrdersApi api) : ControllerBase
{
    private readonly ILogger<EventFoodOrdersControllerParticipant> _logger = logger;
    private readonly EventFoodOrdersApi _api = (EventFoodOrdersApi)api;


    // Participant Start
    // TODO Post /participants/register
    [HttpPost]
    [Route("/participants/register")]
    public IActionResult RegisterForEvent(ParticipantRegistrationRequest ParticipantRegistrationRequest)
    {
        if (ParticipantRegistrationRequest == null)
        {
            return BadRequest();
        }

        User User = _api.GetUser(ParticipantRegistrationRequest.UserId);
        if (User == null)
        {
            return BadRequest("User not found with ID: " + ParticipantRegistrationRequest.UserId.ToString());
        }

        Event _event = _api.GetEvent(ParticipantRegistrationRequest.EventId);
        if (_event == null)
        {
            return BadRequest("Event not found with ID: " + ParticipantRegistrationRequest.EventId.ToString());
        }


        // TODO Fix implementation
        return Ok();
    }

    // TODO Put /participants/update/{id}
    [HttpPut]
    [Route("/participants/update/{id}")]
    public IActionResult UpdateParticipants(ParticipantUpdateRequest ParticipantUpdateRequest)
    {
        if (ParticipantUpdateRequest == null)
        {
            return BadRequest();
        }


        // TODO Fix implementation
        return Ok();
    }

    // TODO Get /participants/events
    [HttpGet]
    [Route("/participants/events")]
    public IActionResult GetAvailableEvents()
    {
        throw new NotImplementedException();
        //        return Ok();
    }

    // TODO Delete /participants/cancel/{id}
    [HttpDelete]
    [Route("/participants/cancel/{id}")]
    public IActionResult cancelRegistration(Guid id)
    {
        throw new NotImplementedException();
        //        return Ok();
    }
}
