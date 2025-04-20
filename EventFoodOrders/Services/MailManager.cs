using EventFoodOrders.Entities;
using EventFoodOrders.Services.Interfaces;

namespace EventFoodOrders.Services;

public class MailManager(IServiceManager sm) : IMailManager
{
    public async Task HandleNewEventMails(
        Event newEvent,
        IEnumerable<Guid> userIds
    )
    {
        if (userIds.Any())
        {
            await sm.MailerService.SendInvitationMail(
                newEvent, userIds.Where(id => id != newEvent.OwnerId));
        }
        await sm.MailerService.SendCreateEventConfirmationMail(newEvent);
    }

    public async Task HandleUpdateEventMails(
        Event updatedEvent,
        IEnumerable<Guid> usersToSendUpdateTo,
        IEnumerable<Guid> usersToSendDeleteTo
    )
    {
        if (usersToSendDeleteTo.Any())
        {
            await sm.MailerService.SendRevokeInvitationMail(
                updatedEvent, usersToSendDeleteTo);
        }

        if (usersToSendUpdateTo.Any())
        {
            await sm.MailerService.SendInvitationUpdateMail(updatedEvent, usersToSendUpdateTo);
        }

        await sm.MailerService.SendUpdateEventConfirmationMail(updatedEvent);
    }

    public async Task HandleCancelEventMails(
        Event eventToDelete,
        Guid ownerId
    )
    {
        var eventUserIds = eventToDelete.Participants
            .Where(p => p.UserId != ownerId)
            .Select(p => p.UserId)
            .ToList();

        await sm.MailerService.SendEventCanceledMail(
            eventToDelete, eventUserIds);

        await sm.MailerService.SendDeleteEventConfirmationMail(eventToDelete, ownerId);
    }
}
