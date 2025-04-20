using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Dto.ParticipantDTOs;
using EventFoodOrders.Dto.UserDTOs;
using EventFoodOrders.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventFoodOrders.Controllers;

[Authorize]
[ApiController]
[Route("/api/event")]
public class EventController(IServiceManager sm) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<EventForResponseDto>> CreateEvent(EventForCreationDto newEvent)
    {
        EventForResponseDto response = await sm.EventService.CreateEvent(sm.IdCarrier.UserId, newEvent);
        return Created(uri: "", value: response);
    }

    [HttpPut]
    [Route("{eventId}")]
    public async Task<ActionResult<EventForResponseDto>> UpdateEvent(Guid eventId, EventForUpdateDto eventToUpdate)
    {
        EventForResponseDto response = await sm.EventService.UpdateEvent(eventId, sm.IdCarrier.UserId, eventToUpdate);
        return Ok(response);
    }

    [HttpDelete]
    [Route("{eventId}")]
    public async Task<ActionResult<bool>> DeleteEvent(Guid eventId)
    {
        bool response = await sm.EventService.DeleteEvent(sm.IdCarrier.UserId, eventId);
        return Ok(response);
    }

    [HttpGet]
    [Route("{eventId}")]
    public async Task<ActionResult<EventForResponseWithDetailsDto>> GetSingleEventForUser(Guid eventId)
    {
        EventForResponseWithDetailsDto response = await sm.EventService.GetEventForUser(sm.IdCarrier.UserId, eventId);
        return Ok(response);
    }

    [HttpGet]
    [Route("{eventId}/info")]
    public async Task<ActionResult<EventForResponseWithUsersDto>> GetSingleEventWithAllParticipantsAndUsers(Guid eventId)
    {
        EventForResponseWithDetailsDto response = await sm.EventService.GetEventForUser(sm.IdCarrier.UserId, eventId);
        IEnumerable<ParticipantForResponseDto> participants = await sm.ParticipantService.GetAllParticipantsForEvent(sm.IdCarrier.UserId, eventId);
        IEnumerable<UserDto> users = await sm.UserService.GetUsersFromIds([.. participants.Select(p => p.UserId)]);
        var dto = sm.EventService.GetEventWithUsers(response, participants, users);
        return Ok(dto);
    }

    [HttpGet]
    [Route("all")]
    public async Task<ActionResult<IEnumerable<EventForResponseDto>>> GetAllEventsForUser()
    {
        IEnumerable<EventForResponseDto> response = await sm.EventService.GetAllEventsForUser(sm.IdCarrier.UserId);
        return Ok(response);
    }
}
