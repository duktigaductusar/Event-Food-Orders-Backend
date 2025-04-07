using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Entities;
using EventFoodOrders.Services.Interfaces;
using EventFoodOrders.Utilities;

namespace EventFoodOrders.Services;

public class MailerService(IUserService userService) : IMailerService
{
    private readonly string baseUrl = $"http://localhost:4200/"; //ToDo: Dynamic URL call from env or config.
    public async Task SendInvitationMail(EventForCreationDto focusedEvent, Guid ownerId, Guid eventId)
    {
        var eventUrl = $"{baseUrl}{eventId}/"; 
        Guid[] ownerIdArray = [ownerId];
        var ownerInfo = await userService.GetUsersFromIds(ownerIdArray);
        var emails = focusedEvent.UserIds.ToList();
        var message = new EmailTemplate(
            "Ibjudan till nytt event",
            $@"<html>
                        <body>
                            <p>{ownerInfo[0].Username} bjuder in dig till {focusedEvent.Title}.</p>
                            <p>{focusedEvent.Description}</p>
                            <p><a href=""{eventUrl}"">Klicka här för att svara på inbjudan.</a></p>
                        </body>
                    </html>");
        await userService.SendEmail(emails, message);
    }

    public async Task SendReminderMail(List<Guid> recipients, Event focusedEvent, Guid eventId)
    {
        var eventUrl = $"{baseUrl}{eventId}/";
        var message = new EmailTemplate(
            $"Påminnelse om {focusedEvent.Title}",
            $@"<html>
                      <body>
                        <p>Deadline för att svara på inbjudan till {focusedEvent.Title} är idag klockan {focusedEvent.Deadline.Hour}:{focusedEvent.Deadline.Minute}</p>
                        <p><a href=""{eventUrl}"">Klicka här för att svara på inbjudan.</a></p>
                      </body>
                    </html>");
        await userService.SendEmail(recipients, message);        
    }
}