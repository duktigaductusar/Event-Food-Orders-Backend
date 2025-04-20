using EventFoodOrders.Services.Interfaces;
using EventFoodOrders.Utilities;

namespace EventFoodOrders.Services;

public class MailerService(
    IConfiguration configuration,
    IServiceManager sm
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
        var ownerInfo = await sm.UserService.GetUsersFromIds(ownerIdArray);

        EmailTemplate message = new(
            "Inbjudan till nytt event",
            DuctusHtmlEmailWrapper($"Inbjudan till event '{Safe(focusedEvent.Title)}'",
                $@"<p style='{EmailStyles.Paragraph}'>{Safe(ownerInfo[0].Username)} bjuder in dig till {Safe(focusedEvent.Title)}.</p>
                <p style='{EmailStyles.Paragraph}'>Event Id: {focusedEvent.Id}</p>
                <p style='{EmailStyles.Paragraph}'>Datum och tid för eventet: {Safe(focusedEvent.Date)}</p>
                <p style='{EmailStyles.Paragraph}'>Deadline: {Safe(focusedEvent.Deadline)}</p>
                <p style='{EmailStyles.Paragraph}'>Antal Deltagare: {userIds.Count()}</p>
                <blockquote style='{EmailStyles.Description}'>{Safe(focusedEvent.Description ?? "Ingen beskrivning tillgänglig")}</blockquote>
                <br/>
                <a style='{EmailStyles.Button}' href=""{Safe(eventUrl)}"">Klicka här för att svara på inbjudan</a>"            
        ));

        await sm.UserService.SendEmail(userIds.ToList(), message);
    }

    public async Task SendInvitationUpdateMail(Entities.Event focusedEvent, IEnumerable<Guid> userIds)
    {
        if (!userIds.Any())
        {
            return;
        }

        var eventUrl = $"{baseUrl}/{userEventPath}/{focusedEvent.Id}";
        Guid[] ownerIdArray = [focusedEvent.OwnerId];
        var ownerInfo = await sm.UserService.GetUsersFromIds(ownerIdArray);

        EmailTemplate message = new(
            "Event har uppdaterats",
            DuctusHtmlEmailWrapper($"Inbjudan till event '{Safe(focusedEvent.Title)}'",
                $@"<p style='{EmailStyles.Paragraph}'>{Safe(ownerInfo[0].Username)} bjuder in dig till {Safe(focusedEvent.Title)}.</p>
                <p style='{EmailStyles.Paragraph}'>Event Id: {focusedEvent.Id}</p>
                <p style='{EmailStyles.Paragraph}'>Datum och tid för eventet: {Safe(focusedEvent.Date)}</p>
                <p style='{EmailStyles.Paragraph}'>Deadline: {Safe(focusedEvent.Deadline)}</p>
                <p style='{EmailStyles.Paragraph}'>Antal Deltagare: {userIds.Count()}</p>
                <blockquote style='{EmailStyles.Description}'>{Safe(focusedEvent.Description ?? "Ingen beskrivning tillgänglig")}</blockquote>
                <br/>
                <a style='{EmailStyles.Button}' href=""{Safe(eventUrl)}"">Klicka här för att svara på inbjudan</a>"
        ));

        await sm.UserService.SendEmail(userIds.ToList(), message);
    }

    public async Task SendEventCanceledMail(
        Entities.Event focusedEvent,
        IEnumerable<Guid> userIds
    )
    {
        if (!userIds.Any()) { return; }
        
        Guid[] ownerIdArray = [focusedEvent.OwnerId];
        var ownerInfo = await sm.UserService.GetUsersFromIds(ownerIdArray);

        EmailTemplate message = new(
            "Eventet har blivit borttaget",
            DuctusHtmlEmailWrapper($"Eventet <strong>{Safe(focusedEvent.Title)}</strong> har ställts in av {Safe(ownerInfo[0].Username)}",
                $@"<p style='{EmailStyles.Paragraph}'>Datum och tid för det borttagna eventet: {Safe(focusedEvent.Date)}</p>
                <p style='{EmailStyles.Paragraph}'>Det innebär att din inbjudan inte längre gäller.</p>
                <p style='{EmailStyles.Paragraph}'>Ingen åtgärd krävs från dig.</p>"
        ));

        await sm.UserService.SendEmail(userIds.ToList(), message);
    }

    public async Task SendRevokeInvitationMail(
        Entities.Event focusedEvent,
        IEnumerable<Guid> userIds
    )
    {
        if (!userIds.Any()) { return; }

        Guid[] ownerIdArray = [focusedEvent.OwnerId];
        var ownerInfo = await sm.UserService.GetUsersFromIds(ownerIdArray);

        EmailTemplate message = new(
            "Din inbjudan har blivit avbokad",
            DuctusHtmlEmailWrapper($"Din inbjudan har blivit avbokad till eventet <strong>{Safe(focusedEvent.Title)}</strong>",
                $@"<p style='{EmailStyles.Paragraph}'>Event Id: {focusedEvent.Id}</p>
                <p style='{EmailStyles.Paragraph}'>Datum för det avbokade eventet: {Safe(focusedEvent.Date)}</p>
                <p style='{EmailStyles.Paragraph}'>{Safe(ownerInfo[0].Username)} har tagit bort dig från deltagarlistan.</p>
                <p style='{EmailStyles.Paragraph}'>Ingen åtgärd krävs från dig.</p>"
        ));

        await sm.UserService.SendEmail(userIds.ToList(), message);
    }

    public async Task SendCreateEventConfirmationMail(Entities.Event focusedEvent)
    {
        var eventUrl = $"{baseUrl}/{adminEventPath}/{focusedEvent.Id}";
        var ownerIdArray = new List<Guid>() { focusedEvent.OwnerId };

        EmailTemplate message = new(
            "Bekräftelse nytt event skapat",
            DuctusHtmlEmailWrapper($"Event '{Safe(focusedEvent.Title)}' har skapats",
                $@"<p style='{EmailStyles.Paragraph}'>Event Id: {focusedEvent.Id}</p>
                <p style='{EmailStyles.Paragraph}'>Datum och tid för eventet: {Safe(focusedEvent.Date)}</p>
                <p style='{EmailStyles.Paragraph}'>Deadline: {Safe(focusedEvent.Deadline)}</p>
                <blockquote style='{EmailStyles.Description}'>{Safe(focusedEvent.Description ?? "Ingen beskrivning tillgänglig")}</blockquote>
                <br/>
                <a style='{EmailStyles.Button}' href=""{Safe(eventUrl)}"">Klicka här för att hantera eventet.</a>"
        ));

        await sm.UserService.SendEmail(ownerIdArray, message);
    }

    public async Task SendUpdateEventConfirmationMail(Entities.Event focusedEvent)
    {
        var eventUrl = $"{baseUrl}/{adminEventPath}/{focusedEvent.Id}";
        var ownerIdArray = new List<Guid>() { focusedEvent.OwnerId };

        EmailTemplate message = new(
            "Bekräftelse event uppdaterat",
            DuctusHtmlEmailWrapper($"Event '{Safe(focusedEvent.Title)}' har uppdaterats",
                $@"<p style='{EmailStyles.Paragraph}'>Event Id: {focusedEvent.Id}</p>
                <p style='{EmailStyles.Paragraph}'>Datum och tid för eventet: {Safe(focusedEvent.Date)}</p>
                <p style='{EmailStyles.Paragraph}'>Deadline: {Safe(focusedEvent.Deadline)}</p>
                <blockquote style='{EmailStyles.Description}'>{Safe(focusedEvent.Description ?? "Ingen beskrivning tillgänglig")}</blockquote>
                <br/>
                <a style='{EmailStyles.Button}' href=""{Safe(eventUrl)}"">Klicka här för att hantera eventet.</a>"
        ));
        await sm.UserService.SendEmail(ownerIdArray, message);
    }

    public async Task SendDeleteEventConfirmationMail(Entities.Event focusedEvent)
    {
        var ownerIdArray = new List<Guid>() { focusedEvent.OwnerId };

        EmailTemplate message = new(
            "Bekräftelse event stängt",
            DuctusHtmlEmailWrapper($"Event '{Safe(focusedEvent.Title)}' har stängts",
                $@"<p style='{EmailStyles.Paragraph}'>Event Id: {focusedEvent.Id}</p>
                <p style='{EmailStyles.Paragraph}'>Datum och tid för det stängda eventet: {Safe(focusedEvent.Date)}</p>
                <p style='{EmailStyles.Paragraph}'>{Safe(focusedEvent.Description ?? String.Empty)}</p>"
        ));
        await sm.UserService.SendEmail(ownerIdArray, message);
    }

    public async Task SendReminderMailConfirmation(Entities.Event focusedEvent, List<Guid> pendingUserIds)
    {
        var eventUrl = $"{baseUrl}/{adminEventPath}/{focusedEvent.Id}";
        var ownerIdArray = new List<Guid>() { focusedEvent.OwnerId };

        EmailTemplate message = new(
            $"Påminelse skickad till väntande deltagare",
            DuctusHtmlEmailWrapper($"Påminelse skickad till väntande deltagare gällande event '{focusedEvent.Title}'",
                $@"<p style='{EmailStyles.Paragraph}'>Event Id: {focusedEvent.Id}</p>
                <p style='{EmailStyles.Paragraph}'>Datum och tid för eventet: {Safe(focusedEvent.Date)}</p>
                <p style='{EmailStyles.Paragraph}'>Deadline: {Safe(focusedEvent.Deadline)}</p>
                <p style='{EmailStyles.Paragraph}'>Antal väntande svar: {pendingUserIds.Count}</p>    
                <blockquote style='{EmailStyles.Description}'>{Safe(focusedEvent.Description ?? "Ingen beskrivning tillgänglig")}</blockquote>
                <br/>
                <a style='{EmailStyles.Button}' href=""{Safe(eventUrl)}"">Klicka här för att hantera eventet.</a>"
        ));
        await sm.UserService.SendEmail(ownerIdArray, message);
    }

    public async Task SendReminderMail(List<Guid> recipients, Entities.Event focusedEvent)
    {
        if (recipients.Count == 0) { return; }
        var eventUrl = $"{baseUrl}/{userEventPath}/{focusedEvent.Id}/";

        EmailTemplate message = new(
            $"Påminnelse om {Safe(focusedEvent.Title)}",
            DuctusHtmlEmailWrapper($"Event '{Safe(focusedEvent.Title)}' påminelse",
                $@"<p style='{EmailStyles.Paragraph}'>Event Id: {focusedEvent.Id}</p>
                <p style='{EmailStyles.Paragraph}'>Datum och tid för eventet: {Safe(focusedEvent.Date)}</p>
                <p style='{EmailStyles.Paragraph}'>Deadline: {Safe(focusedEvent.Deadline)}</p>
                <blockquote style='{EmailStyles.Description}'>{Safe(focusedEvent.Description ?? "Ingen beskrivning tillgänglig")}</blockquote>
                <br/>
                <a style='{EmailStyles.Button}' href=""{eventUrl}"">Klicka här för att svara på inbjudan.</a>"
         ));
        await sm.UserService.SendEmail(recipients, message);
    }

    public async Task SendSummaryMail(Entities.Event focusedEvent)
    {
        List<Guid> ownerId = [focusedEvent.OwnerId];
        var office = focusedEvent.Participants.Where(p => p.ResponseType == ReType.AttendingOffice).ToList();
        var wantsFood = focusedEvent.Participants.Where(p => p.WantsMeal).ToList();

        var allergies = focusedEvent.Participants
            .Select(p => p.Allergies?.Trim())
            .Where(a => !string.IsNullOrWhiteSpace(a))
            .ToHashSet();

        var preferences = focusedEvent.Participants
            .Select(p => p.Preferences?.Trim())
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .ToHashSet();

        var allergiesString = string.Join(", ", allergies);
        var preferencesString = string.Join(", ", preferences);

        var allergiesHtml = string.IsNullOrWhiteSpace(allergiesString)
            ? string.Empty
            : $"<p style='{EmailStyles.Paragraph}'>Allergier: {Safe(allergiesString)}</p>";

        var preferencesHtml = string.IsNullOrWhiteSpace(preferencesString)
            ? string.Empty
            : $"<p style='{EmailStyles.Paragraph}'>Matpreferenser: {Safe(preferencesString)}</p>";

        var eventUrl = $"{baseUrl}/{adminEventPath}/{focusedEvent.Id}/";

        EmailTemplate message = new(
            $"Sammanfattning för {Safe(focusedEvent.Title)}",
            DuctusHtmlEmailWrapper($"Deadline för {Safe(focusedEvent.Title)} har gått ut",
                $@"<p style='{EmailStyles.Paragraph}'>Event Id: {focusedEvent.Id}</p>
                <p style='{EmailStyles.Paragraph}'>Datum och tid för eventet: {Safe(focusedEvent.Date.LocalDateTime)}</p>
                <p style='{EmailStyles.Paragraph}'>{office.Count} deltagare kommer närvara på plats.</p>
                <p style='{EmailStyles.Paragraph}'>{wantsFood.Count} deltagare önskar mat, {allergies.Count} deltagare har anmält allergier och {preferences.Count} deltagare har anmält matpreferenser.</p>        
                {allergiesHtml}
                {p}
                <blockquote style='{EmailStyles.Description}'>{Safe(focusedEvent.Description ?? "Ingen beskrivning tillgänglig")}</blockquote>
                <br/>
                <a style='{EmailStyles.Button}' href=""{Safe(eventUrl)}"">Klicka här för att hantera eventet.</a>"
         ));
        await sm.UserService.SendEmail(ownerId, message);
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

    private static class EmailStyles
    {
        public const string Body = "margin: 0; padding: 8px; background-color: #f5f5f5;";
        public const string Wrapper = "max-width: 600px; font-family: Arial, sans-serif; color: #333; background-color: white; padding: 0; margin: 0 auto; border: 1px solid #dae5e1; border-radius: 16px;";
        public const string Header = "padding: 20px; background-color: #dae5e1; border-top-left-radius: 16px; border-top-right-radius: 16px;";
        public const string Main = "padding: 30px; background-color: white;";
        public const string Footer = "padding: 20px; background-color: #dae5e1; border-bottom-left-radius: 16px; border-bottom-right-radius: 16px;";
        public const string Button = "display: inline-block; background: #64837a; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;";
        public const string Heading = "font-size: 20px; font-weight: bold; margin-bottom: 10px;";
        public const string Paragraph = "margin-bottom: 10px;";
        public const string Description = "margin: 16px 0; padding: 12px 16px; background-color: #f9f9f9; border-left: 4px solid #64837a; color: #444; font-style: italic;";
        public const string Icon = @"<img src=""https://ductus.global/wp-content/uploads/2023/12/ductus-logo-black.png"" alt=""Ductus"" width=""320"" style=""max-width: 100%; height: auto; display: block; margin: 0 auto;"" />";
    }

    private static string DuctusHtmlEmailWrapper(string title, string content)
    {
        return $@"
<html>
    <body style='{EmailStyles.Body}'>
        <div style='{EmailStyles.Wrapper}'>
            <div style='{EmailStyles.Header}'>
                <h3 style='{EmailStyles.Heading}'>{title}</h3>
            </div>
            <div style='{EmailStyles.Main}'>
                {content}
            </div>
            <div style='{EmailStyles.Footer}'>
                {EmailStyles.Icon}
            </div>
        </div>
    </body>
</html>";
    }
}