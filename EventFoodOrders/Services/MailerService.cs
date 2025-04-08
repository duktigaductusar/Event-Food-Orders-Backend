using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Entities;
using EventFoodOrders.Services.Interfaces;
using EventFoodOrders.Utilities;

namespace EventFoodOrders.Services;

public class MailerService(IUserService userService) : IMailerService
{
    private readonly string baseUrl = $"http://localhost:4200/"; //ToDo: Dynamic URL call from env or config.
    
    //ToDo: eventId parameter needed? Should be findable in focusedEvent entity.
    public async Task SendInvitationMail(EventForCreationDto focusedEvent, Guid ownerId, Guid eventId)
    {
        var eventUrl = $"{baseUrl}{eventId}/"; 
        Guid[] ownerIdArray = [ownerId];
        var ownerInfo = await userService.GetUsersFromIds(ownerIdArray);
        var emails = focusedEvent.UserIds.ToList();
        EmailTemplate message = new(
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
        EmailTemplate message = new(
            $"Påminnelse om {focusedEvent.Title}",
            $@"<html>
                      <body>
                        <p>Deadline för att svara på inbjudan till {focusedEvent.Title} är idag klockan {focusedEvent.Deadline.Hour}:{focusedEvent.Deadline.Minute}</p>
                        <p><a href=""{eventUrl}"">Klicka här för att svara på inbjudan.</a></p>
                      </body>
                    </html>");
        await userService.SendEmail(recipients, message);        
    }

    public async Task SendSummaryMail(Event focusedEvent)
    {
        List<Guid> ownerId = [focusedEvent.OwnerId];
        var office = focusedEvent.Participants.Where(p => p.ResponseType == ReType.AttendingOffice).ToList();
        var wantsFood = focusedEvent.Participants.Where(p => p.WantsMeal).ToList();
        var allergies = focusedEvent.Participants.Where(p => p.Allergies != null).Select(p => p.Allergies).ToHashSet();
        var allergiesString = string.Join(", ", allergies);
        var preferences = focusedEvent.Participants.Where(p => p.Preferences != null).Select(p => p.Preferences).ToHashSet();
        var preferencesString = string.Join(", ", preferences);
        EmailTemplate message = new(
            $"Sammanfattning för {focusedEvent.Title}",
            $@"<html>
                      <body>
                        <p>Deadline för {focusedEvent.Title} har gått ut.</p>
                        <p>{office.Count} personer kommer närvara på plats.</p>
                        <p>{wantsFood.Count} personer önskar mat, {allergies.Count} person(er) har anmält allergier och {preferences.Count} person(er) har anmält matpreferenser.</p>
                        <p></p>
                        <p>Allergier: {allergiesString}</p>
                        <p></p>
                        <p>Matpreferenser: {preferencesString}</p>
                      </body>
                    </html>");
        await userService.SendEmail(ownerId, message);
        
    }
}