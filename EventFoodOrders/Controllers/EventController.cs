using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventFoodOrders.Controllers;

[ApiController]
[Route("/event")]
public class EventController(ILogger logger, EventService service) : ControllerBase
{
    private readonly ILogger _logger = logger;
    private readonly EventService _service = service;

    [HttpPost]
    [Route("/new")]
    public ActionResult<EventForResponseDto> CreateEvent(EventForCreationDto newEvent)
    {
        EventForResponseDto response = _service.CreateEvent(newEvent);

        return Ok(response);
    }

    [HttpPut]
    [Route("/update/{eventId}")]
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
    [Route("/delete/{eventId}")]
    public ActionResult<bool> DeleteEvent(string eventId)
    {
        try { Guid.Parse(eventId); }
        catch { return BadRequest("Id not valid Guid."); }

        try
        {
            bool response = _service.DeleteEvent(eventId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    //[Route("/get/{userId}/{eventId}")]
    [Route("/get/{eventId}")]
    public ActionResult<EventForResponseDto> GetSingleEventForUser(string eventId)
    {
        //ToDo: Update how the controller gets the user id, this is a temp Guid as string
        string userId = "4aa80da1-69dc-449f-bf4c-be8daafcef2a";

        try{Guid.Parse(eventId);}
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
    [Route("/get/all")]
    public ActionResult<IEnumerable<EventForResponseDto>> GetAllEventsForUser()
    {
        //ToDo: Update how the controller gets the user id, this is a temp Guid as string
        string userId = "4aa80da1-69dc-449f-bf4c-be8daafcef2a";
        IEnumerable<EventForResponseDto> response = _service.GetAllEventsForUser(userId);

        return Ok(response);
    }
}
