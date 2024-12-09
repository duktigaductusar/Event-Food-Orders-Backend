using EventFoodOrders.Api;
using EventFoodOrders.Dto;
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
    public IActionResult RegisterForEvent(ParticipantRegistrationRequestDTO _participantRegistrationRequest)
    {
        if (_participantRegistrationRequest == null)
        {
            return BadRequest();
        }

        User user = _api.GetUser(_participantRegistrationRequest.userId);
        if (user == null)
        {
            return BadRequest("User not found with ID: " + _participantRegistrationRequest.userId.ToString());
        }

        Models.Event _event = _api.GetEvent(_participantRegistrationRequest.eventId);
        if (_event == null)
        {
            return BadRequest("Event not found with ID: " + _participantRegistrationRequest.eventId.ToString());
        }


        // TODO Fix implementation
        return Ok();
    }

    // TODO Put /participants/update/{id}
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

    // TODO new method, getParticipantIdByUserIdAndEventId. Check java impl
    /*
    @GetMapping("/get-participant/{userId}/{eventId}")
    public ResponseEntity<Map<String, UUID>> getParticipantIdByUserIdAndEventId(@PathVariable UUID userId, @PathVariable UUID eventId)
    {
        return participantService.findParticipantIdByUserIdAndEventId(userId, eventId)
                .map(participantId->ResponseEntity.ok(Collections.singletonMap("participantId", participantId)))
                .orElse(ResponseEntity.status(404).body(Collections.singletonMap("participantId", null)));
    }
    */

    // TODO new method, getParticipantDetails. Check java impl
    /*
    @GetMapping("/details/{participantId}")
    public ResponseEntity<ParticipantDTO> getParticipantDetails(@PathVariable UUID participantId)
    {
        ParticipantDTO participantDetails = participantService.getParticipantDetails(participantId);
        return ResponseEntity.ok(participantDetails);
    }

    */
}
