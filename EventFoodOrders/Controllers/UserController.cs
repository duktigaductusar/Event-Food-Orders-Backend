using EventFoodOrders.Dto.UserDTOs;
using EventFoodOrders.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventFoodOrders.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IUserService userService)
{
    private readonly IUserService _userService = userService;

    [HttpGet]
    public ActionResult<List<UserDto>> GetUsersFromQuery(string queryString)
    {
        var users = _userService.GetUsersFromQuery(queryString);
        return users;
    }

    [HttpPost]
    [Route("userId")]
    public ActionResult<List<UserDto>> GetUsers([FromBody] UserIdsDto userIds)
    {
        var users = _userService.GetUsersFromIds(userIds.UserIds);
        return users;
    }
}
