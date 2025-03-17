using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Dto.ParticipantDTOs;
using EventFoodOrders.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
    public ActionResult<ParticipantForResponseDto> AddParticipantToEvent(string eventId, ParticipantForCreationDto newParticipant)
    {
        ParticipantForResponseDto response = _participantService.AddParticipantToEvent(eventId, newParticipant);

        return Ok(response);
    }

    [HttpPut]
    [Route("/update/{participantId}")]
    public ActionResult<ParticipantForResponseDto> UpdateParticipant(string participantId, ParticipantForUpdateDto participantToUpdate)
    {
        ParticipantForResponseDto response = _participantService.UpdateParticipant(participantId, participantToUpdate);

        return Ok();
    }

    [HttpDelete]
    [Route("/delete/{eventId}/{participantId}")]
    public ActionResult<bool> DeleteParticipant(string eventId, string participantId)
    {
        return Ok();
    }

    [HttpGet]
    //[Route("/get/{userId}/{eventId}")]
    [Route("/get/{eventId}/{participantId}")]
    public ActionResult<EventForResponseDto> GetSingleParticipantInEvent(string eventId, string participantId)
    {
        return Ok();
    }

    [HttpGet]
    //[Route("/get/{userId}/all")]
    [Route("/get/{eventId}/all")]
    public ActionResult<IEnumerable<EventForResponseDto>> GetAllParticipantsInEvent(string eventId)
    {
        return Ok();
    }
}
