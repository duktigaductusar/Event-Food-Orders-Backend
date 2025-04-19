using EventFoodOrders.Services.Interfaces;
using EventFoodOrders.Utilities;

namespace EventFoodOrders.Services;

public class MailerService(
    IConfiguration configuration,
    IUserService userService
) : IMailerService
{
    private readonly string baseUrl = configuration["ClientBaseUrl"]
        ?? throw new ArgumentNullException($"Environment variable 'BaseUrl' is missing in");
    private readonly string adminEventPath = "event-management";
    private readonly string userEventPath = "event-details";

    public async Task SendInvitationMail(Entities.Event focusedEvent, IEnumerable<Guid> userIds)
    {
        if (!userIds.Any())
        {
            return;
        }

        var eventUrl = $"{baseUrl}/{userEventPath}/{focusedEvent.Id}";
        Guid[] ownerIdArray = [focusedEvent.OwnerId];
        var ownerInfo = await userService.GetUsersFromIds(ownerIdArray);

        EmailTemplate message = new(
            "Inbjudan till nytt event",
            $@"<html>
                <body>
                    <p>{Safe(ownerInfo[0].Username)} bjuder in dig till {Safe(focusedEvent.Title)}.</p>
                    <p>Event Id: {focusedEvent.Id}</p>
                    <p>Datum och tid för eventet: {Safe(focusedEvent.Date)}</p>
                    <p>Deadline: {Safe(focusedEvent.Deadline)}</p>
                    <p>Antal Deltagare: {userIds.Count()}</p>
                    <p>{Safe(focusedEvent.Description ?? String.Empty)}</p>
                    <p><a href=""{Safe(eventUrl)}"">Klicka här för att svara på inbjudan.</a></p>
                </body>
            </html>");

        await userService.SendEmail(userIds.ToList(), message);
    }

    public async Task SendInvitationUpdateMail(Entities.Event focusedEvent, IEnumerable<Guid> userIds)
    {
        if (!userIds.Any())
        {
            return;
        }

        var eventUrl = $"{baseUrl}/{userEventPath}/{focusedEvent.Id}";
        Guid[] ownerIdArray = [focusedEvent.OwnerId];
        var ownerInfo = await userService.GetUsersFromIds(ownerIdArray);

        EmailTemplate message = new(
            "Event har uppdaterats",
            $@"<html>
                <body>
                    <p>{Safe(ownerInfo[0].Username)} bjuder in dig till {Safe(focusedEvent.Title)}.</p>
                    <p>Event Id: {focusedEvent.Id}</p>
                    <p>Datum och tid för eventet: {Safe(focusedEvent.Date)}<p>
                    <p>Deadline: {Safe(focusedEvent.Deadline)}</p>
                    <p>Antal Deltagare: {userIds.Count()}</p>
                    <p>{Safe(focusedEvent.Description ?? String.Empty)}</p>
                    <p><a href=""{Safe(eventUrl)}"">Klicka här för att svara på inbjudan.</a></p>
                </body>
            </html>");

        await userService.SendEmail(userIds.ToList(), message);
    }

    public async Task SendEventCanceledMail(
        Entities.Event focusedEvent,
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
                <p>Eventet <strong>{Safe(focusedEvent.Title)}</strong> har ställts av {Safe(ownerInfo[0].Username)}.</p>
                <p>Datum och tid för det borttagna eventet: {Safe(focusedEvent.Date)}</p>
                <p>Det innebär att din inbjudan inte längre gäller.</p>
                <p>Ingen åtgärd krävs från dig.</p>
            </body>
        </html>");

        await userService.SendEmail(userIds.ToList(), message);
    }

    public async Task SendRevokeInvitationMail(
        Entities.Event focusedEvent,
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
                <p>Din inbjudan har blivit avbokad till eventet <strong>{Safe(focusedEvent.Title)}</strong>.</p>
                <p>Event Id: {focusedEvent.Id}</p>
                <p>Datum för det avbokade eventet: {Safe(focusedEvent.Date)}</p>
                <p>{Safe(ownerInfo[0].Username)} har tagit bort dig från deltagarlistan.</p>
                <p>Ingen åtgärd krävs från dig.</p>
            </body>
        </html>");

        await userService.SendEmail(userIds.ToList(), message);
    }

    public async Task SendCreateEventConfirmationMail(Entities.Event focusedEvent)
    {
        var eventUrl = $"{baseUrl}/{adminEventPath}/{focusedEvent.Id}";
        var ownerIdArray = new List<Guid>() { focusedEvent.OwnerId };
        EmailTemplate message = new(
            "Bekräftelse nytt event skapat",
            $@"<html>
                <body>
                    <p>Event '{Safe(focusedEvent.Title)}' har skapats.</p>
                    <p>Event Id: {focusedEvent.Id}</p>
                    <p>Datum och tid för eventet: {Safe(focusedEvent.Date)}</p>
                    <p>Deadline: {Safe(focusedEvent.Deadline)}</p>
                    <p>{Safe(focusedEvent.Description ?? String.Empty)}</p>
                    <p><a href=""{Safe(eventUrl)}"">Klicka här för att hantera eventet.</a></p>
                </body>
            </html>");
        await userService.SendEmail(ownerIdArray, message);
    }

    public async Task SendUpdateEventConfirmationMail(Entities.Event focusedEvent)
    {
        var eventUrl = $"{baseUrl}/{adminEventPath}/{focusedEvent.Id}";
        var ownerIdArray = new List<Guid>() { focusedEvent.OwnerId };
        EmailTemplate message = new(
            "Bekräftelse event uppdaterat",
            $@"<html>
                <body>
                    <p>Event '{Safe(focusedEvent.Title)}' har uppdaterats.</p>
                    <p>Event Id: {focusedEvent.Id}</p>
                    <p>Datum och tid för eventet: {Safe(focusedEvent.Date)}</p>
                    <p>Deadline: {Safe(focusedEvent.Deadline)}</p>
                    <p>{Safe(focusedEvent.Description ?? String.Empty)}</p>   
                    <p><a href=""{Safe(eventUrl)}"">Klicka här för att hantera eventet.</a></p>
                </body>
            </html>");
        await userService.SendEmail(ownerIdArray, message);
    }

    public async Task SendDeleteEventConfirmationMail(Entities.Event focusedEvent, Guid ownerId)
    {
        var ownerIdArray = new List<Guid>() { ownerId };
        EmailTemplate message = new(
            "Bekräftelse event stängt",
            $@"<html>
                <body>
                    <p>Event '{Safe(focusedEvent.Title)}' har stängts.</p>
                    <p>Event Id: {focusedEvent.Id}</p>
                    <p>Datum och tid för det stängda eventet: {Safe(focusedEvent.Date)}</p>
                    <p>{Safe(focusedEvent.Description ?? String.Empty)}</p>
                </body>
            </html>");
        await userService.SendEmail(ownerIdArray, message);
    }

    public async Task SendReminderMail(List<Guid> recipients, Entities.Event focusedEvent)
    {
        if (recipients.Count == 0) { return; }

        var eventUrl = $"{baseUrl}/{userEventPath}/{focusedEvent.Id}/";
        EmailTemplate message = new(
            $"Påminnelse om {Safe(focusedEvent.Title)}",
            $@"<html>
                <body>
                    <p>Event '{Safe(focusedEvent.Title)}' påminelse.</p>
                    <p>Event Id: {focusedEvent.Id}</p>
                    <p>Datum och tid för eventet: {Safe(focusedEvent.Date)}</p>
                    <p>Deadline: {Safe(focusedEvent.Deadline)}</p>
                    <p><a href=""{eventUrl}"">Klicka här för att svara på inbjudan.</a></p>
                 </body>
            </html>");
        await userService.SendEmail(recipients, message);
    }

    public async Task SendSummaryMail(Entities.Event focusedEvent)
    {
        List<Guid> ownerId = [focusedEvent.OwnerId];
        var office = focusedEvent.Participants.Where(p => p.ResponseType == ReType.AttendingOffice).ToList();
        var wantsFood = focusedEvent.Participants.Where(p => p.WantsMeal).ToList();
        var allergies = focusedEvent.Participants.Where(p => p.Allergies != null).Select(p => p.Allergies).ToHashSet();
        var allergiesString = string.Join(", ", allergies);
        var preferences = focusedEvent.Participants.Where(p => p.Preferences != null).Select(p => p.Preferences).ToHashSet();
        var preferencesString = string.Join(", ", preferences);
        var eventUrl = $"{baseUrl}/{adminEventPath}/{focusedEvent.Id}/";

        EmailTemplate message = new(
            $"Sammanfattning för {Safe(focusedEvent.Title)}",
            $@"<html>
                <body>
                    <p>Deadline för {Safe(focusedEvent.Title)} har gått ut.</p>
                    <p>Event Id: {focusedEvent.Id}</p>
                    <p>Datum och tid för eventet: {Safe(focusedEvent.Date.LocalDateTime)}</p>
                    <p>{office.Count} personer kommer närvara på plats.</p>
                    <p>{wantsFood.Count} personer önskar mat, {allergies.Count} person(er) har anmält allergier och {preferences.Count} person(er) har anmält matpreferenser.</p>
                    <p>Allergier: {Safe(allergiesString)}</p>
                    <p>Matpreferenser: {Safe(preferencesString)}</p>
                    <p><a href=""{Safe(eventUrl)}"">Klicka här för att hantera eventet.</a></p>
                </body>
            </html>");
        await userService.SendEmail(ownerId, message);

    }

    private static string Safe(string unsafeValue)
    {
        return System.Net.WebUtility.HtmlEncode(unsafeValue);
    }

    private static string Safe(DateTimeOffset unsafeValue)
    {
        return System.Net.WebUtility.HtmlEncode(
            DateUtility.GetSwedishDateTimeOffsetAsString(unsafeValue)
        );
    }
}