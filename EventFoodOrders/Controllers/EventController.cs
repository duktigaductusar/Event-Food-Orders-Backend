using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Dto.ParticipantDTOs;
using EventFoodOrders.Dto.UserDTOs;
using EventFoodOrders.IdHandling;
using EventFoodOrders.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventFoodOrders.Controllers;

[Authorize]
[ApiController]
[Route("/api/event")]
public class EventController(IServiceManager serviceManager, IIdCarrier carrier) : ControllerBase
{
    private readonly IEventService _service = serviceManager.EventService;
    private readonly IIdCarrier _carrier = carrier;

    [HttpPost]
    public async Task<ActionResult<EventForResponseDto>> CreateEvent(EventForCreationDto newEvent)
    {
        EventForResponseDto response = await _service.CreateEvent(_carrier.UserId, newEvent);
        return Created(uri: "", value: response);
    }

    [HttpPut]
    [Route("{eventId}")]
    public async Task<ActionResult<EventForResponseDto>> UpdateEvent(Guid eventId, EventForUpdateDto eventToUpdate)
    {
        EventForResponseDto response = await _service.UpdateEvent(eventId, _carrier.UserId, eventToUpdate);
        return Ok(response);
    }

    [HttpDelete]
    [Route("{eventId}")]
    public async Task<ActionResult<bool>> DeleteEvent(Guid eventId)
    {
        bool response = await _service.DeleteEvent(_carrier.UserId, eventId);
        return Ok(response);
    }

    [HttpGet]
    [Route("{eventId}")]
    public async Task<ActionResult<EventForResponseWithDetailsDto>> GetSingleEventForUser(Guid eventId)
    {
        EventForResponseWithDetailsDto response = await _service.GetEventForUser(_carrier.UserId, eventId);
        return Ok(response);
    }

    [HttpGet]
    [Route("{eventId}/info")]
    public async Task<ActionResult<EventForResponseWithUsersDto>> GetSingleEventWithAllParticipantsAndUsers(Guid eventId)
    {
        EventForResponseWithDetailsDto response = await _service.GetEventForUser(_carrier.UserId, eventId);
        IEnumerable<ParticipantForResponseDto> participants = await serviceManager.ParticipantService.GetAllParticipantsForEvent(_carrier.UserId, eventId);
        IEnumerable<UserDto> users = await serviceManager.UserService.GetUsersFromIds([.. participants.Select(p => p.UserId)]);
        var dto = _service.GetEventWithUsers(response, participants, users);
        return Ok(dto);
    }

    [HttpGet]
    [Route("all")]
    public async Task<ActionResult<IEnumerable<EventForResponseDto>>> GetAllEventsForUser()
    {
        IEnumerable<EventForResponseDto> response = await _service.GetAllEventsForUser(_carrier.UserId);
        return Ok(response);
    }
}
