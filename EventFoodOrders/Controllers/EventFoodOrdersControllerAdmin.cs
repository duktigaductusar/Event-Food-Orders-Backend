

using EventFoodOrders.Api;
using EventFoodOrders.Dto;
using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventFoodOrders.Controllers;
/*
[ApiController]
public class EventFoodOrdersControllerAdmin(ILogger<EventFoodOrdersControllerAdmin> logger, IEventFoodOrdersApi api) : ControllerBase
{
    private readonly ILogger<EventFoodOrdersControllerAdmin> _logger = logger;
    private readonly EventFoodOrdersApi _api = (EventFoodOrdersApi)api;

    [HttpPost]
    [Route("/admin/users")]
    public IActionResult AddUsers(UserDTO _user)
    {
        if (_user == null)
        {
            return BadRequest();
        }

        User user = new();

        return Ok(user);
    }

    [HttpDelete]
    [Route("/admin/users/{id}")]
    public IActionResult DeleteUsers(String id)
    {
        Guid _id;

        try
        {
            _id = new Guid(id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        _api.DeleteUser(_id);

        return Ok();
    }

    [HttpPost]
    [Route("/admin/events")]
    public IActionResult AddEvent(EventForResponseDTO _event)
    {
        Event retVal = _api.SaveEvent(_event);

        return Ok(retVal);
    }

    [HttpPut]
    [Route("/admin/events/{id}")]
    public IActionResult UpdateEvent(string id, Event _event)
    {
        Event retVal = _api.UpdateEvent(id, _event);

        return Ok(retVal);
    }

    [HttpDelete]
    [Route("/admin/events/{id}")]
    public IActionResult DeleteEvent(string id)
    {
        try
        {
            _api.DeleteEvent(new Guid(id));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok();
    }

    [HttpGet]
    [Route("/admin/events/{eventId}/participants-with-meal")]
    public IActionResult getEventWithMeal(string eventId)
    {
        List<Participant> participants = _api.getEventWithMeal(new Guid(eventId));
        if (participants == null || participants.Count <= 0)
        {
            return NotFound();
        }

        return Ok(participants);
    }
}
*/