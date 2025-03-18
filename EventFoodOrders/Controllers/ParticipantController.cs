using System.Collections.Generic;
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
        bool response = _participantService.DeleteParticipant(participantId);

        return Ok();
    }

    [HttpGet]
    //[Route("/get/{userId}/{eventId}")]
    [Route("/get/{eventId}/{participantId}")]
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
    [Route("/get/{eventId}/all")]
    public ActionResult<IEnumerable<ParticipantForResponseDto>> GetAllParticipantsInEvent(string eventId)
    {
        IEnumerable <ParticipantForResponseDto> participants = _participantService.GetAllParticipantsForEvent(eventId);
        return Ok(participants);
    }
}
