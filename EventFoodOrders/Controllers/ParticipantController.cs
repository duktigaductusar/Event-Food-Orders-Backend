using EventFoodOrders.Dto.ParticipantDTOs;
using EventFoodOrders.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EventFoodOrders.Controllers;

//[Authorize] //Un-comment when ready for full auth flow
[ApiController]
[Route("/api/participant")]
public class ParticipantController(IServiceManager serviceManager) : ControllerBase
{
    private readonly IParticipantService _participantService = serviceManager.ParticipantService;

    [HttpPost]
    [Route("{eventId}")]
    public ActionResult<ParticipantForResponseDto> AddParticipantToEvent(Guid eventId, ParticipantForCreationDto newParticipant)
    {
        ParticipantForResponseDto response = _participantService.AddParticipantToEvent(eventId, newParticipant);
        return Created(uri: "", value: response);
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
        ParticipantForResponseDto response = _participantService.GetParticipant(participantId, eventId);
        return Ok(response);
    }

    [HttpGet]
    //[Route("/get/{userId}/all")]
    [Route("{eventId}/all")]
    public ActionResult<IEnumerable<ParticipantForResponseDto>> GetAllParticipantsInEvent(Guid userId, Guid eventId)
    {
        IEnumerable <ParticipantForResponseDto> response = _participantService.GetAllParticipantsForEvent(userId, eventId);
        return Ok(response);
    }
}
