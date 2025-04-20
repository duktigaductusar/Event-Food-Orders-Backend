using EventFoodOrders.Dto.ParticipantDTOs;
using EventFoodOrders.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EventFoodOrders.Controllers;

[Authorize]
[ApiController]
[Route("/api/participant")]
public class ParticipantController(IServiceManager sm) : ControllerBase
{
    [HttpPut]
    [Route("{participantId}")]
    public async Task<ActionResult<ParticipantForResponseDto>> UpdateParticipant(Guid participantId, ParticipantForUpdateDto dto)
    {
        ParticipantForResponseDto response = await sm.ParticipantService.UpdateParticipant(participantId, dto);
        return Ok(response);
    }

    [HttpPut]
    [Route("{participantId}/response-type")]
    public async Task<ActionResult<ParticipantForResponseDto>> UpdateParticipantResponse(Guid participantId, ParticipantForUpdateResponseTypeDto dto)
    {
        ParticipantForResponseDto response = await sm.ParticipantService.UpdateParticipantResponseType(participantId, dto);
        return Ok(response);
    }

    [HttpGet]
    [Route("{eventId}/all")]
    public async Task<ActionResult<IEnumerable<ParticipantForResponseDto>>> GetAllParticipantsInEvent(Guid eventId)
    {
        IEnumerable <ParticipantForResponseDto> response = await sm.ParticipantService.GetAllParticipantsForEvent(sm.IdCarrier.UserId, eventId);
        return Ok(response);
    }
}
