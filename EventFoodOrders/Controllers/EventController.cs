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
    [Route("/update/{id}")]
    public ActionResult<EventForResponseDTO> UpdateEvent(string id, EventForUpdateDto eventToUpdate)
    {
        EventForResponseDTO response = _service.UpdateEvent(id, eventToUpdate);

        return Ok(response);
    }

    [HttpDelete]
    [Route("/delete/{id}")]
    public ActionResult<bool> DeleteEvent(string id)
    {
        bool response = _service.DeleteEvent(id);

        return Ok(response);
    }

    [HttpGet]
    [Route("/get/{userId}/{eventId}")]
    public ActionResult<EventForResponseDTO> GetSingleEventForUser(string userId, string eventId)
    {
        EventForResponseDTO response = _service.GetEventForUser(userId, eventId);

        return Ok(response);
    }

    [HttpGet]
    [Route("/get/{userId}/all")]
    public ActionResult<IEnumerable<EventForResponseDTO>> GetAllEventsForUser(string userId)
    {
        IEnumerable<EventForResponseDTO> response = _service.GetAllEventsForUser(userId);

        return Ok(response);
    }
}
