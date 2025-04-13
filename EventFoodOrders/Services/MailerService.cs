using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Entities;
using EventFoodOrders.Services.Interfaces;
using EventFoodOrders.Utilities;
using Microsoft.Extensions.Logging;

namespace EventFoodOrders.Services;

/**
 * TODO! HTML encode or use template engine (.NET Razor) to prevent HTML injection.
 * E.g.:      
 *       var safeTitle = System.Net.WebUtility.HtmlEncode(focusedEvent.Title);
 *       var safeDescription = System.Net.WebUtility.HtmlEncode(focusedEvent.Description);
 *       var safeUsername = System.Net.WebUtility.HtmlEncode(ownerInfo[0].Username);
 */
public class MailerService(IUserService userService) : IMailerService
{
    private readonly string baseUrl = $"http://localhost:4200/"; //ToDo: Dynamic URL call from env or config.

    //ToDo: Link not working, Frontend redirects to Home page.
    public async Task SendInvitationMail(EventForCreationDto focusedEvent, Guid ownerId, Guid eventId)
    {
        var userIds = focusedEvent.UserIds?.ToList() ?? [];
        if (userIds.Count == 0)
        {
            return;
        }

        var eventUrl = $"{baseUrl}event-details/{eventId}";
        Guid[] ownerIdArray = [ownerId];
        var ownerInfo = await userService.GetUsersFromIds(ownerIdArray);

        EmailTemplate message = new(
            "Inbjudan till nytt event",
            $@"<html>
                <body>
                    <p>{ownerInfo[0].Username} bjuder in dig till {focusedEvent.Title}.</p>
                    <p>Antal Deltagare: {userIds.Count}</p>
                    <br/>
                    <p>{focusedEvent.Description}</p>
                    <p><a href=""{eventUrl}"">Klicka här för att svara på inbjudan.</a></p>
                </body>
            </html>");

        await userService.SendEmail(userIds, message);
    }

    public async Task SendInvitationMail(EventForUpdateDto focusedEvent, Guid ownerId, Guid eventId)
    {
        var userIds = focusedEvent.UserIds?.ToList() ?? [];
        if (userIds.Count == 0)
        {
            return;
        }

        var eventUrl = $"{baseUrl}event-details/{eventId}";
        Guid[] ownerIdArray = [ownerId];
        var ownerInfo = await userService.GetUsersFromIds(ownerIdArray);

        EmailTemplate message = new(
            "Event har uppdaterats",
            $@"<html>
                <body>
                    <p>{ownerInfo[0].Username} bjuder in dig till {focusedEvent.Title}.</p>
                    <p>Antal Deltagare: {userIds.Count}</p>
                    <br/>
                    <p>{focusedEvent.Description}</p>
                    <p><a href=""{eventUrl}"">Klicka här för att svara på inbjudan.</a></p>
                </body>
            </html>");

        await userService.SendEmail(userIds, message);
    }

    public async Task SendEventCanceledMail(
        Event focusedEvent,
        IEnumerable<Guid> userIds
    )
    {
        if (!userIds.Any()) { return; }
        
        Guid[] ownerIdArray = [focusedEvent.OwnerId];
        var ownerInfo = await userService.GetUsersFromIds(ownerIdArray);

        EmailTemplate message = new(
        "Eventet har blivit borttaget",
        $@"<html>
            <body>
                <p>Eventet <strong>{focusedEvent.Title}</strong> har ställts av {ownerInfo[0].Username}.</p>
                <p>Datum för det borttagna eventet: {focusedEvent.Date.LocalDateTime}<p>
                <br/>
                <p>Det innebär att din inbjudan inte längre gäller.</p>
                <p>Ingen åtgärd krävs från dig.</p>
            </body>
        </html>");

        await userService.SendEmail(userIds.ToList(), message);
    }

    public async Task SendRevokeInvitationMail(
        Event focusedEvent,
        IEnumerable<Guid> userIds
    )
    {
        if (!userIds.Any()) { return; }

        Guid[] ownerIdArray = [focusedEvent.OwnerId];
        var ownerInfo = await userService.GetUsersFromIds(ownerIdArray);

        EmailTemplate message = new(
        "Din inbjudan har blivit avbokad",
        $@"<html>
            <body>
                <p>Din inbjudan har blivit avbokad till eventet <strong>{focusedEvent.Title}</strong>.</p>
                <p>Datum för det avbokade eventet: {focusedEvent.Date.LocalDateTime}<p>
                <br/>
                <p>{ownerInfo[0].Username} har tagit bort dig från deltagarlistan.</p>
                <p>Ingen åtgärd krävs från dig.</p>
            </body>
        </html>");

        await userService.SendEmail(userIds.ToList(), message);
    }

    public async Task SendCreateEventConfirmationMail(EventForCreationDto focusedEvent, Guid ownerId, Guid eventId)
    {
        var eventUrl = $"{baseUrl}event-management/{eventId}";
        var ownerIdArray = new List<Guid>() { ownerId };
        EmailTemplate message = new(
            "Bekräftelse nytt event skapat",
            $@"<html>
                <body>
                    <p>Event '{focusedEvent.Title}' har skapats.</p>
                    <p>{focusedEvent.Description}</p>
                    <p>Event Id: {eventId}</p>
                    <br/>
                    <p><a href=""{eventUrl}"">Klicka här för att hantera eventet.</a></p>
                </body>
            </html>");
        await userService.SendEmail(ownerIdArray, message);
    }

    public async Task SendUpdateEventConfirmationMail(EventForUpdateDto focusedEvent, Guid ownerId, Guid eventId)
    {
        var eventUrl = $"{baseUrl}event-management/{eventId}";
        var ownerIdArray = new List<Guid>() { ownerId };
        EmailTemplate message = new(
            "Bekräftelse event uppdaterat",
            $@"<html>
                <body>
                    <p>Event '{focusedEvent.Title}' har uppdaterats.</p>
                    <p>{focusedEvent.Description}</p>   
                    <p>Event Id: {eventId}</p>
                    <br/>
                    <p><a href=""{eventUrl}"">Klicka här för att hantera eventet.</a></p>
                </body>
            </html>");
        await userService.SendEmail(ownerIdArray, message);
    }

    public async Task SendDeleteEventConfirmationMail(Event focusedEvent, Guid ownerId)
    {
        var ownerIdArray = new List<Guid>() { ownerId };
        EmailTemplate message = new(
            "Bekräftelse event stängt",
            $@"<html>
                <body>
                    <p>Event '{focusedEvent.Title}' har stängts.</p>
                    <p>Event Id: {focusedEvent.Id}</p>
                    <p>Datum för det stängda eventet: {focusedEvent.Date.LocalDateTime}<p>
                    <br/>
                    <p>{focusedEvent.Description}</p>
                    <p>Event Id: {focusedEvent.Id}</p>
                </body>
            </html>");
        await userService.SendEmail(ownerIdArray, message);
    }

    public async Task SendReminderMail(List<Guid> recipients, Event focusedEvent, Guid eventId)
    {
        if (recipients.Count == 0) { return; }

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