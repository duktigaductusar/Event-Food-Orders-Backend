using EventFoodOrders.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventFoodOrders.Controllers;

public class MailController(IServiceManager serviceManager) : ControllerBase
{
    [HttpPost("mail")]
    public async Task<IActionResult> Mail(Guid[] userIds)
    {
        try
        {
            await serviceManager.UserService.SendEmail(userIds);
            return Ok("Emails sent");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error when sending mail: {ex.Message}");
        }
    }
}