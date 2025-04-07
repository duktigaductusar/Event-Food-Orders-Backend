using EventFoodOrders.Dto.UserDTOs;
using EventFoodOrders.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EventFoodOrders.Controllers;

//[Authorize] //Un-comment when ready for full auth flow
[ApiController]
[Route("api/user")]
public class UserController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<UserDto[]>> GetUsersFromQuery(string queryString, [FromQuery] Guid eventId)
    {
        Guid userId = serviceManager.AuthService.GetUserIdFromUserClaims(User.Claims);
        //var users = await serviceManager.UserService.GetUsersFromQuery(queryString);

        var users = await serviceManager.UserService.GetUsersFromQuery(queryString, eventId);
        users.RemoveAll(u => u.UserId == userId);
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
