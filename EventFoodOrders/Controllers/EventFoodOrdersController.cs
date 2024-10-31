using EventFoodOrders.Api;
using EventFoodOrders.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventFoodOrders.Controllers
{
    [ApiController]
    public class EventFoodOrdersController(ILogger<EventFoodOrdersController> logger, IEventFoodOrdersApi api) : ControllerBase
    {
        private readonly ILogger<EventFoodOrdersController> _logger = logger;
        private readonly EventFoodOrdersApi _api = (EventFoodOrdersApi)api;

        [HttpGet]
        [Route("/api/dummies")]
        public IActionResult Get()
        {
            List<Dummy> dummies = _api.GetDummies().ToList<Dummy>();

            return Ok(dummies);
        }

        [HttpGet]
        [Route("/api/dummy/{id}")]
        public IActionResult Get(string id)
        {
            Dummy dummy = _api.GetDummy(id);

            return Ok(dummy);
        }

        [HttpPost]
        [Route("/api/add/dummy")]
        public IActionResult newDummy(Dummy dummy)
        {
            return Ok(_api.AddDummy(dummy));
        }

        [HttpPut]
        [Route("/api/update/dummy")]
        public IActionResult UpdateDummy(Dummy dummy)
        {
            return Ok(_api.UpdateDummy(dummy));
        }

        [HttpDelete]
        [Route("/api/delete/dummy/{id}")]
        public IActionResult DeleteDummy(string id)
        {
            _api.DeleteDummy(id);

            return Ok();
        }



    }
}
