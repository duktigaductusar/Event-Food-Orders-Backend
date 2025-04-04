using EventFoodOrders.Dto.EventDTOs;
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
        EventForResponseDto response = _service.UpdateEvent(eventId, eventToUpdate);
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
    //[Route("/get/{userId}/{eventId}")]
    [Route("{eventId}")]
    public ActionResult<EventForResponseWithDetailsDto> GetSingleEventForUser(Guid eventId, Guid userId)
    {
        EventForResponseWithDetailsDto response = _service.GetEventForUser(userId, eventId);
        return Ok(response);
    }

    [HttpGet]
    //[Route("/get/{userId}/all")]
    [Route("all")]
    public ActionResult<IEnumerable<EventForResponseDto>> GetAllEventsForUser()
    {
        //ToDo: Update how the controller gets the user id, this is a temp Guid as string
        Guid userId = Guid.Parse("a84c12d5-9075-42d2-b467-6b345b7d8c9f");
        IEnumerable<EventForResponseDto> response = _service.GetAllEventsForUser(userId);

        return Ok(response);
    }
}
