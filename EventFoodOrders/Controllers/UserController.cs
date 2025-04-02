using EventFoodOrders.Dto.UserDTOs;
using EventFoodOrders.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventFoodOrders.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<UserDto[]>> GetUsersFromQuery(string queryString)
    {
        var users = await serviceManager.UserService.GetUsersFromQuery(queryString);
        return users;
    }

    [HttpPost]
    [Route("userId")]
    public async Task<ActionResult<UserDto[]>> GetUsers([FromBody] UserIdsDto userIds)
    {
        var users = await serviceManager.UserService.GetUsersFromIds(userIds.UserIds);
        return users;
    }
    
    [HttpGet("find")]
    public async Task<IActionResult> SearchUserByName(string search)
    {
        try
        {
            var foundUsers = await serviceManager.UserService.GetUsersFromQuery(search);
            return Ok(foundUsers);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error when searching users by string: {ex.Message}");
        }
    }
}
