using EventFoodOrders.Dto.UserDTOs;
using EventFoodOrders.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventFoodOrders.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    public ActionResult<UserDto[]> GetUsersFromQuery(string queryString)
    {
        Guid userId = serviceManager.AuthService.GetUserIdFromUserClaims(User.Claims);
        //var users = await serviceManager.UserService.GetUsersFromQuery(queryString);

        var users = serviceManager.UserService.GetUsersFromQuery(queryString);
        users.RemoveAll(u => u.UserId == userId);
        return users.ToArray();
    }

    [HttpPost]
    [Route("userId")]
    public ActionResult<UserDto[]> GetUsers([FromBody] UserIdsDto userIds)
    {
        //var users = await serviceManager.UserService.GetUsersFromIds(userIds.UserIds);
        var users = serviceManager.UserService.GetUsersFromIds(userIds.UserIds);
        return users.ToArray();
    }
}
