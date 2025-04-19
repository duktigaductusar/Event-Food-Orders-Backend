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
        public const string Icon = @"<img src=""https://ductus.global/wp-content/uploads/2023/12/ductus-logo-black.png"" alt=""Ductus"" width=""320"" style=""max-width: 100%; height: auto; display: block; margin: 0 auto;"" />";
    }

    private static string HtmlWrapper(string title, string content)
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
            HtmlWrapper($"Inbjudan till event '{Safe(focusedEvent.Title)}'",
                $@"<p style='{EmailStyles.Paragraph}'>{Safe(ownerInfo[0].Username)} bjuder in dig till {Safe(focusedEvent.Title)}.</p>
                <p style='{EmailStyles.Paragraph}'>Event Id: {focusedEvent.Id}</p>
                <p style='{EmailStyles.Paragraph}'>Datum och tid för eventet: {Safe(focusedEvent.Date)}</p>
                <p style='{EmailStyles.Paragraph}'>Deadline: {Safe(focusedEvent.Deadline)}</p>
                <p style='{EmailStyles.Paragraph}'>Antal Deltagare: {userIds.Count()}</p>
                <p style='{EmailStyles.Paragraph}'>{Safe(focusedEvent.Description ?? String.Empty)}</p>
                <br/>
                <a style='{EmailStyles.Button}' href=""{Safe(eventUrl)}"">Klicka här för att svara på inbjudan</a>"            
        ));

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
            HtmlWrapper($"Inbjudan till event '{Safe(focusedEvent.Title)}'",
                $@"<p style='{EmailStyles.Paragraph}'>{Safe(ownerInfo[0].Username)} bjuder in dig till {Safe(focusedEvent.Title)}.</p>
                <p style='{EmailStyles.Paragraph}'>Event Id: {focusedEvent.Id}</p>
                <p style='{EmailStyles.Paragraph}'>Datum och tid för eventet: {Safe(focusedEvent.Date)}</p>
                <p style='{EmailStyles.Paragraph}'>Deadline: {Safe(focusedEvent.Deadline)}</p>
                <p style='{EmailStyles.Paragraph}'>Antal Deltagare: {userIds.Count()}</p>
                <p style='{EmailStyles.Paragraph}'>{Safe(focusedEvent.Description ?? String.Empty)}</p>
                <br/>
                <a style='{EmailStyles.Button}' href=""{Safe(eventUrl)}"">Klicka här för att svara på inbjudan</a>"
        ));

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
            HtmlWrapper($"Inbjudan till event '{Safe(focusedEvent.Title)}'",
                $@"<p style='{EmailStyles.Paragraph}'>Eventet <strong>{Safe(focusedEvent.Title)}</strong> har ställts av {Safe(ownerInfo[0].Username)}.</p>
                <p style='{EmailStyles.Paragraph}'>Datum och tid för det borttagna eventet: {Safe(focusedEvent.Date)}</p>
                <p style='{EmailStyles.Paragraph}'>Det innebär att din inbjudan inte längre gäller.</p>
                <p style='{EmailStyles.Paragraph}'>Ingen åtgärd krävs från dig.</p>"
        ));

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