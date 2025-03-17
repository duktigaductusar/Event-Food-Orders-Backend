using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Dto.ParticipantDTOs;
using EventFoodOrders.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventFoodOrders.Controllers;

[ApiController]
[Route("/participant")]
public class ParticipantController(ILogger logger, ParticipantService participantService, EventService eventService) : ControllerBase
{
    private readonly ILogger _logger = logger;
    private readonly EventService _eventService = eventService;
    private readonly ParticipantService _participantService = participantService;

    [HttpPost]
    [Route("/{eventId}/new")]
    public ActionResult<ParticipantForResponseDto> AddParticipantToEvent(string eventId, ParticipantForCreationDto newEvent)
    {
        ParticipantForResponseDto response = _participantService.AddParticipantToEvent(eventId, newEvent);

        return Ok(response);
    }

    [HttpPut]
    [Route("/update/{participantId}")]
    public ActionResult<ParticipantForResponseDto> UpdateParticipant(string participantId, ParticipantForUpdateDto participantToUpdate)
    {


        return Ok();
    }

    [HttpDelete]
    [Route("/delete/{eventId}/8888")]
    public ActionResult<bool> DeleteEvent(string eventId)
    {
        return Ok();
    }

    [HttpGet]
    //[Route("/get/{userId}/{eventId}")]
    [Route("/get/{eventId}/5555")]
    public ActionResult<EventForResponseDto> GetSingleEventForUser(string eventId)
    {
        return Ok();
    }

    [HttpGet]
    //[Route("/get/{userId}/all")]
    [Route("/get/all/7777")]
    public ActionResult<IEnumerable<EventForResponseDto>> GetAllEventsForUser()
    {
        

        return Ok();
    }
}
