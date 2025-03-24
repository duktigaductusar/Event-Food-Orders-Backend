using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventFoodOrders.Controllers;

[ApiController]
[Route("/api/event")]
public class EventController(EventService service) : ControllerBase
{
    private readonly EventService _service = service;

    [HttpPost]
    public ActionResult<EventForResponseDto> CreateEvent(Guid userId, EventForCreationDto newEvent)
    {
        EventForResponseDto response = _service.CreateEvent(userId, newEvent);
        return Ok(response);
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
        bool response = false;

        try
        {
            response = _service.DeleteEvent(eventId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(response);
    }

    [HttpGet]
    //[Route("/get/{userId}/{eventId}")]
    [Route("{eventId}")]
    public ActionResult<EventForResponseWithDetailsDto> GetSingleEventForUser(Guid eventId, Guid userId)
    {
        try
        {
            EventForResponseWithDetailsDto response = _service.GetEventForUser(userId, eventId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    //[Route("/get/{userId}/all")]
    [Route("all")]
    public ActionResult<IEnumerable<EventForResponseDto>> GetAllEventsForUser()
    {
        //ToDo: Update how the controller gets the user id, this is a temp Guid as string
        Guid userId = Guid.NewGuid();
        IEnumerable<EventForResponseDto> response = _service.GetAllEventsForUser(userId);

        return Ok(response);
    }
}
