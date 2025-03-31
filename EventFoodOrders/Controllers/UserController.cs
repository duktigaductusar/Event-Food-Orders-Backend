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
    public ActionResult<List<UserDto>> GetUsers(string queryString)
    {
        var users = _userService.GetUsers(queryString);
        return users;
    }
}
