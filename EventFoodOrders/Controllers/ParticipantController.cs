using EventFoodOrders.Dto.ParticipantDTOs;
using EventFoodOrders.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventFoodOrders.Controllers;

[ApiController]
[Route("/api/participant")]
public class ParticipantController(ParticipantService participantService) : ControllerBase
{
    private readonly ParticipantService _participantService = participantService;

    [HttpPost]
    [Route("{eventId}")]
    public ActionResult<ParticipantForResponseDto> AddParticipantToEvent(Guid eventId, ParticipantForCreationDto newParticipant)
    {
        ParticipantForResponseDto response;

        try
        {
            response = _participantService.AddParticipantToEvent(eventId, newParticipant);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(response);
    }

    [HttpPut]
    [Route("{participantId}")]
    public ActionResult<ParticipantForResponseDto> UpdateParticipant(Guid participantId, ParticipantForUpdateDto participantToUpdate)
    {
        ParticipantForResponseDto response = _participantService.UpdateParticipant(participantId, participantToUpdate);

        return Ok(response);
    }

    [HttpDelete]
    [Route("{participantId}")]
    public ActionResult<bool> DeleteParticipant(Guid participantId)
    {
        bool response = _participantService.DeleteParticipant(participantId);

        return Ok(response);
    }

    [HttpGet]
    //[Route("/get/{userId}/{eventId}")]
    [Route("{participantId}")]
    public ActionResult<ParticipantForResponseDto> GetSingleParticipantInEvent(Guid eventId, Guid participantId)
    {
        ParticipantForResponseDto response;

        try
        {
            response = _participantService.GetParticipant(participantId, eventId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(response);
    }

    [HttpGet]
    //[Route("/get/{userId}/all")]
    [Route("{eventId}/all")]
    public ActionResult<IEnumerable<ParticipantForResponseDto>> GetAllParticipantsInEvent(Guid userId, Guid eventId)
    {
        IEnumerable <ParticipantForResponseDto> response;

        try
        {
            response = _participantService.GetAllParticipantsForEvent(userId, eventId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(response);
    }
    //ToDo: Remove unused?
    /*
    [HttpGet]
    //[Route("/get/{userId}/all")]
    [Route("{userId}/all")]
    public ActionResult<IEnumerable<ParticipantForResponseDto>> GetAllParticipantsForUser(string userId)
    {
        IEnumerable<ParticipantForResponseDto> response = _participantService.GetAllParticipantsForUser(userId);

        return Ok(response);
    }*/
}
