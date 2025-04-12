using EventFoodOrders.Dto.ParticipantDTOs;
using EventFoodOrders.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EventFoodOrders.IdHandling;

namespace EventFoodOrders.Controllers;

//[Authorize] //ToDo: Un-comment when ready for full auth flow
[ApiController]
[Route("/api/participant")]
public class ParticipantController(IServiceManager serviceManager, IIdCarrier carrier) : ControllerBase
{
    private readonly IParticipantService _participantService = serviceManager.ParticipantService;
    private readonly IIdCarrier _carrier = carrier;

    [HttpPost]
    [Route("{eventId}")]
    public ActionResult<ParticipantForResponseDto> AddParticipantToEvent(Guid eventId, ParticipantForCreationDto dto)
    {
        ParticipantForResponseDto response = _participantService.AddParticipantToEvent(eventId, dto);
        return Created(uri: "", value: response);
    }

    [HttpPut]
    [Route("{participantId}")]
    public ActionResult<ParticipantForResponseDto> UpdateParticipant(Guid participantId, ParticipantForUpdateDto dto)
    {
        ParticipantForResponseDto response = _participantService.UpdateParticipant(participantId, dto);
        return Ok(response);
    }

    [HttpPut]
    [Route("{participantId}/response-type")]
    public ActionResult<ParticipantForResponseDto> UpdateParticipantResponse(Guid participantId, ParticipantForUpdateResponseTypeDto dto)
    {
        ParticipantForResponseDto response = _participantService.UpdateParticipantResponseType(participantId, dto);
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
    [Route("{participantId}")]
    public ActionResult<ParticipantForResponseDto> GetSingleParticipantInEvent(Guid eventId, Guid participantId)
    {
        ParticipantForResponseDto response = _participantService.GetParticipant(participantId, eventId);
        return Ok(response);
    }

    [HttpGet]
    [Route("{eventId}/all")]
    public ActionResult<IEnumerable<ParticipantForResponseDto>> GetAllParticipantsInEvent(Guid eventId)
    {
        IEnumerable <ParticipantForResponseDto> response = _participantService.GetAllParticipantsForEvent(_carrier.UserId, eventId);
        return Ok(response);
    }
}
