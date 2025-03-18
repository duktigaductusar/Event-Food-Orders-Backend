using EventFoodOrders.Dto.ParticipantDTOs;
using EventFoodOrders.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventFoodOrders.Controllers;

[ApiController]
[Route("/api/")]
public class ParticipantController(ILogger<ParticipantController> logger, ParticipantService participantService, EventService eventService) : ControllerBase
{
    private readonly ILogger<ParticipantController> _logger = logger;
    private readonly EventService _eventService = eventService;
    private readonly ParticipantService _participantService = participantService;

    [HttpPost]
    [Route("[controller]/{eventId}/new")]
    public ActionResult<ParticipantForResponseDto> AddParticipantToEvent(string eventId, ParticipantForCreationDto newParticipant)
    {
        ParticipantForResponseDto response = _participantService.AddParticipantToEvent(eventId, newParticipant);

        return Ok(response);
    }

    [HttpPut]
    [Route("[controller]/update/{participantId}")]
    public ActionResult<ParticipantForResponseDto> UpdateParticipant(string participantId, ParticipantForUpdateDto participantToUpdate)
    {
        ParticipantForResponseDto response = _participantService.UpdateParticipant(participantId, participantToUpdate);

        return Ok();
    }

    [HttpDelete]
    [Route("[controller]/delete/{eventId}/{participantId}")]
    public ActionResult<bool> DeleteParticipant(string eventId, string participantId)
    {
        bool response = _participantService.DeleteParticipant(participantId);

        return Ok();
    }

    [HttpGet]
    //[Route("/get/{userId}/{eventId}")]
    [Route("[controller]/get/{eventId}/{participantId}")]
    public ActionResult<ParticipantForResponseDto> GetSingleParticipantInEvent(string eventId, string participantId)
    {
        try
        {
            ParticipantForResponseDto response = _participantService.GetParticipant(participantId, eventId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok();
    }

    [HttpGet]
    //[Route("/get/{userId}/all")]
    [Route("[controller]/get/{userId}/{eventId}/all")]
    public ActionResult<IEnumerable<ParticipantForResponseDto>> GetAllParticipantsInEvent(string userId, string eventId)
    {
        IEnumerable <ParticipantForResponseDto> participants = _participantService.GetAllParticipantsForEvent(userId, eventId);
        return Ok(participants);
    }
}
