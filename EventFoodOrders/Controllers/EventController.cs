using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventFoodOrders.Controllers;

[ApiController]
[Route("/event")]
public class EventController(ILogger<EventFoodOrdersControllerAdmin> logger, EventService service) : ControllerBase
{
    private readonly ILogger<EventFoodOrdersControllerAdmin> _logger = logger;
    private readonly EventService _service = service;

    [HttpPost]
    [Route("/new")]
    public ActionResult<EventForResponseDTO> CreateEvent(EventForCreationDto newEvent)
    {
        EventForResponseDTO response = _service.CreateEvent(newEvent);

        return Ok(response);
    }

    [HttpPut]
    [Route("/update/{eventId}")]
    public ActionResult<EventForResponseDTO> UpdateEvent(string eventId, EventForUpdateDto eventToUpdate)
    {
        EventForResponseDTO response = _service.UpdateEvent(eventId, eventToUpdate);

        return Ok(response);
    }

    [HttpDelete]
    [Route("/delete/{eventId}")]
    public ActionResult<bool> DeleteEvent(string eventId)
    {
        bool response = _service.DeleteEvent(eventId);

        return Ok(response);
    }

    [HttpGet]
    //[Route("/get/{userId}/{eventId}")]
    [Route("/get/{eventId}")]
    public ActionResult<EventForResponseDTO> GetSingleEventForUser(string eventId)
    {
        //ToDo: Update how the controller gets the user id, this is a temp Guid as string
        string userId = "4aa80da1-69dc-449f-bf4c-be8daafcef2a";
        EventForResponseDTO response = _service.GetEventForUser(userId, eventId);

        return Ok(response);
    }

    [HttpGet]
    //[Route("/get/{userId}/all")]
    [Route("/get/all")]
    public ActionResult<IEnumerable<EventForResponseDTO>> GetAllEventsForUser()
    {
        //ToDo: Update how the controller gets the user id, this is a temp Guid as string
        string userId = "4aa80da1-69dc-449f-bf4c-be8daafcef2a";
        IEnumerable<EventForResponseDTO> response = _service.GetAllEventsForUser(userId);

        return Ok(response);
    }
}
