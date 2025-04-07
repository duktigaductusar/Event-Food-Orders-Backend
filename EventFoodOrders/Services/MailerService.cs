using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Entities;
using EventFoodOrders.Services.Interfaces;
using EventFoodOrders.Utilities;

namespace EventFoodOrders.Services;

public class MailerService(IUserService userService) : IMailerService
{
    private readonly string baseUrl = $"http://localhost:4200/"; //ToDo: Dynamic URL call from env or config.
    public async Task SendInvitationMail(EventForCreationDto focusedEvent, Guid ownerId)
    {
        var eventUrl = $"{baseUrl}{focusedEvent}/"; 
        Guid[] ownerIdArray = [ownerId];
        var ownerInfo = await userService.GetUsersFromIds(ownerIdArray);
        var emails = focusedEvent.UserIds.ToList();
        var message = new EmailTemplate(
            "Ibjudan till nytt event",
            $"{ownerInfo[0].Username} bjuder in dig till {focusedEvent.Title}.\n {focusedEvent.Description} \n <p><a href=\"{eventUrl}\">Klicka här för att svara på inbjudan.</a></p>");
        await userService.SendEmail(emails, message);
    }

    public async Task SendReminderMail(List<Guid> recipients, Event focusedEvent)
    {
        var eventUrl = $"{baseUrl}{focusedEvent}/";
        var message = new EmailTemplate(
            $"Påminnelse om {focusedEvent.Title}",
            $"Deadline för att svara på ibjudan till {focusedEvent.Title} är idag klockan {focusedEvent.Deadline.Hour}:{focusedEvent.Deadline.Minute} \n <p><a href=\"{eventUrl}\">Klicka här för att svara på inbjudan.</a></p>");
        await userService.SendEmail(recipients, message);        
    }
}