using EventFoodOrders.Api;
using EventFoodOrders.Dto;
using EventFoodOrders.Exceptions;
using EventFoodOrders.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventFoodOrders.Controllers;

[ApiController]
public class EventFoodOrdersControllerParticipant(ILogger<EventFoodOrdersControllerParticipant> logger, IEventFoodOrdersApi api) : ControllerBase
{
    private readonly ILogger<EventFoodOrdersControllerParticipant> _logger = logger;
    private readonly EventFoodOrdersApi _api = (EventFoodOrdersApi)api;

    [HttpPost]
    [Route("/participants/register")]
    public IActionResult RegisterForEvent(ParticipantRegistrationRequestDTO _participantRegistrationRequest)
    {

        ParticipantDTO retVal;
        try
        {
            retVal = _api.RegisterForEvent(_participantRegistrationRequest);
        }
        catch (Exception ex) when (ex is BadRequestException || ex is ArgumentException)
        {
            return BadRequest(ex.Message);
        }

        return Ok(retVal);
    }

    [HttpPut]
    [Route("/participants/update/{id}")]
    public IActionResult UpdateParticipants(ParticipantUpdateRequestDTO _participantRegistrationRequest)
    {
        if (_participantRegistrationRequest == null)
        {
            return BadRequest();
        }


        // TODO Fix implementation
        return Ok();
    }

    [HttpGet]
    [Route("/participants/events")]
    public IActionResult GetAvailableEvents()
    {
        List<Event> retVal = new();

        try
        {
            retVal = _api.GetAvailableEvents();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }

        return Ok(retVal);

    }

    [HttpDelete]
    [Route("/participants/cancel/{id}")]
    public IActionResult cancelRegistration(Guid id)
    {
        _api.cancelRegistration(id);
        return Ok();
    }

    [HttpGet]
    [Route("/participants/get-participant/{userId}/{eventId}")]
    public IActionResult getParticipantIdByUserIdAndEventId(Guid userId, Guid eventId)
    {
        var participantId = _api.findParticipantByUserIdAndEventId(userId, eventId);
        if (participantId != null)
        {
            var dict = new Dictionary<string, Guid>();
            dict.Add("participantId", participantId.participant_id);
            var retVal = Ok(participantId.participant_id);
            return retVal;
        }
        else
        {
            return StatusCode(404);
        }
    }

    [HttpGet]
    [Route("/participants/details/{participantId}")]
    public IActionResult getParticipantDetails(Guid participantId)
    {
        return Ok(_api.getParticipantDetails(participantId));
    }
}
