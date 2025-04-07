using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Dto.ParticipantDTOs;
using EventFoodOrders.Dto.UserDTOs;
using EventFoodOrders.Exceptions;
using Microsoft.AspNetCore.Authorization;
using EventFoodOrders.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models;

namespace EventFoodOrders.Controllers;

//[Authorize] //Un-comment when ready for full auth flow
[ApiController]
[Route("/api/event")]
public class EventController(IServiceManager serviceManager) : ControllerBase
{
    private readonly IEventService _service = serviceManager.EventService;

    [HttpPost]
    public ActionResult<EventForResponseDto> CreateEvent(EventForCreationDto newEvent)
    {
        Guid userId = serviceManager.AuthService.GetUserIdFromUserClaims(User.Claims);
        EventForResponseDto response = _service.CreateEvent(userId, newEvent);
        return Created(uri: "", value: response);
    }

    [HttpPut]
    [Route("{eventId}")]
    public ActionResult<EventForResponseDto> UpdateEvent(Guid eventId, EventForUpdateDto eventToUpdate)
    {
        Guid userId = serviceManager.AuthService.GetUserIdFromUserClaims(User.Claims);
        EventForResponseDto response = _service.UpdateEvent(eventId, userId, eventToUpdate);
        return Ok(response);
    }

    [HttpDelete]
    [Route("{eventId}")]
    public ActionResult<bool> DeleteEvent(Guid eventId)
    {
        bool response = _service.DeleteEvent(eventId);
        return Ok(response);
    }

    [HttpGet]
    [Route("{eventId}")]
    public ActionResult<EventForResponseWithDetailsDto> GetSingleEventForUser(Guid eventId)
    {
        Guid userId = serviceManager.AuthService.GetUserIdFromUserClaims(User.Claims);
        EventForResponseWithDetailsDto response = _service.GetEventForUser(userId, eventId);
        return Ok(response);
    }

    [HttpGet]
    [Route("{eventId}/info")]
    public async Task<ActionResult<EventForResponseWithUsersDto>> GetSingleEventWithAllParticipantsAndUsers(Guid eventId)
    {
        Guid userId = serviceManager.AuthService.GetUserIdFromUserClaims(User.Claims);
        EventForResponseWithDetailsDto response = _service.GetEventForUser(userId, eventId);
        IEnumerable<ParticipantForResponseDto> participants = serviceManager.ParticipantService.GetAllParticipantsForEvent(userId, eventId);
        IEnumerable<UserDto> users = await serviceManager.UserService.GetUsersFromIds([.. participants.Select(p => p.UserId)]);
        var dto = _service.GetEventWithUsers(response, participants, users);
        return Ok(dto);
    }

    [HttpGet]
    [Route("all")]
    public ActionResult<IEnumerable<EventForResponseDto>> GetAllEventsForUser()
    {
        Guid userId = serviceManager.AuthService.GetUserIdFromUserClaims(User.Claims);
        IEnumerable<EventForResponseDto> response = _service.GetAllEventsForUser(userId);

        return Ok(response);
    }
}
