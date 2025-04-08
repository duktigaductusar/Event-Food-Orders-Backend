using EventFoodOrders.Dto.UserDTOs;
using EventFoodOrders.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EventFoodOrders.IdHandling;

namespace EventFoodOrders.Controllers;

//[Authorize] //Un-comment when ready for full auth flow
[ApiController]
[Route("api/user")]
public class UserController(IServiceManager serviceManager, IIdCarrier carrier) : ControllerBase
{
    private readonly IIdCarrier _carrier = carrier;

    [HttpGet]
    public async Task<ActionResult<UserDto[]>> GetUsersFromQuery(string queryString)
    {
        var users = await serviceManager.UserService.GetUsersFromQuery(queryString);
        users.RemoveAll(u => u.UserId == _carrier.UserId);
        return users.ToArray();
    }

    [HttpPost]
    [Route("userId")]
    public async Task<ActionResult<UserDto[]>> GetUsers([FromBody] UserIdsDto userIds)
    {
        //var users = await serviceManager.UserService.GetUsersFromIds(userIds.UserIds);
        List<UserDto> users = await serviceManager.UserService.GetUsersFromIds(userIds.UserIds);
        return users.ToArray();
    }
}
