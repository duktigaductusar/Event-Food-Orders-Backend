using EventFoodOrders.Dto.UserDTOs;
using EventFoodOrders.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EventFoodOrders.Controllers;

[Authorize]
[ApiController]
[Route("api/user")]
public class UserController(IServiceManager sm) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<UserDto[]>> GetUsersFromQuery([FromQuery] string queryString, [FromQuery] Guid? eventId)
    {   
        var users = await sm.UserService.GetUsersFromQuery(queryString, eventId);
        users.RemoveAll(u => u.UserId == sm.IdCarrier.UserId);
        return users.ToArray();
    }

    [HttpPost]
    [Route("userId")]
    public async Task<ActionResult<UserDto[]>> GetUsers([FromBody] UserIdsDto userIds)
    {
        var users = await sm.UserService.GetUsersFromIds(userIds.UserIds);
        return users.ToArray();
    }
}
