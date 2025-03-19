using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventFoodOrders.Controllers;

[ApiController]
[Route("/api/")]
public class EventController(ILogger<EventController> logger, EventService service) : ControllerBase
{
    private readonly ILogger<EventController> _logger = logger;
    private readonly EventService _service = service;

    [HttpPost]
    [Route("[controller]/new")]
    public ActionResult<EventForResponseDto> CreateEvent(EventForCreationDto newEvent)
    {
        EventForResponseDto response = _service.CreateEvent(newEvent);

        return Ok(response);
    }

    [HttpPut]
    [Route("[controller]/update/{eventId}")]
    public ActionResult<EventForResponseDto> UpdateEvent(string eventId, EventForUpdateDto eventToUpdate)
    {
        try { Guid.Parse(eventId); }
        catch { return BadRequest("Id not valid Guid."); }

        try
        {
            EventForResponseDto response = _service.UpdateEvent(eventId, eventToUpdate);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [Route("[controller]/delete/{eventId}")]
    public ActionResult<bool> DeleteEvent(string eventId)
    {
        try { Guid.Parse(eventId); }
        catch { return BadRequest("Id not valid Guid."); }

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
    [Route("[controller]/get/{eventId}")]
    public ActionResult<EventForResponseDto> GetSingleEventForUser(string eventId, string userId)
    {
        try { Guid.Parse(userId); }
        catch { return BadRequest("Id not valid Guid."); }

        try { Guid.Parse(eventId); }
        catch{return BadRequest("Id not valid Guid.");}

        try
        {
            EventForResponseDto response = _service.GetEventForUser(userId, eventId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    //[Route("/get/{userId}/all")]
    [Route("[controller]/get/all")]
    public ActionResult<IEnumerable<EventForResponseDto>> GetAllEventsForUser()
    {
        //ToDo: Update how the controller gets the user id, this is a temp Guid as string
        string userId = "4aa80da1-69dc-449f-bf4c-be8daafcef2a";
        IEnumerable<EventForResponseDto> response = _service.GetAllEventsForUser(userId);

        return Ok(response);
    }
}
