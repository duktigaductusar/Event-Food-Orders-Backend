using EventFoodOrders.Dto.EventDTOs;
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
        IEnumerable<Participant> participantsToSendUpdateTo,
        IEnumerable<Participant> participantsToSendDeleteTo
    )
    {
        var userIds = participantsToSendUpdateTo
                .Select(p => p.UserId)
                .ToHashSet();

        var userIdsToSendRevokeMailTo = participantsToSendDeleteTo
            .Where(p => !userIds.Contains(p.UserId))
            .Select(p => p.UserId)
            .ToHashSet();

        if (userIdsToSendRevokeMailTo.Count != 0)
        {
            await sm.MailerService.SendRevokeInvitationMail(
                updatedEvent, userIdsToSendRevokeMailTo);
        }

        if (userIds.Count != 0)
        {
            await sm.MailerService.SendInvitationUpdateMail(updatedEvent, userIds);
        }

        await sm.MailerService.SendUpdateEventConfirmationMail(updatedEvent);
    }

    public async Task HandleCancelEventMails(
        Event eventToDelete,
        Guid ownerId
    )
    {
        var eventParticipants = eventToDelete.Participants
            .Where(p => p.UserId != ownerId)
            .Select(p => p.UserId)
            .ToList();

        await sm.MailerService.SendEventCanceledMail(
            eventToDelete, eventParticipants);

        await sm.MailerService.SendDeleteEventConfirmationMail(eventToDelete, ownerId);
    }
}
