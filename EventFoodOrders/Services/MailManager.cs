using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Entities;
using EventFoodOrders.Services.Interfaces;

namespace EventFoodOrders.Services;

public class MailManager(IMailerService mailerService) : IMailManager
{
    public async Task HandleNewEventMails(
        Event newEvent,
        IEnumerable<Guid> userIds
    )
    {
        if (userIds.Any())
        {
            await mailerService.SendInvitationMail(newEvent, userIds);
        }
        await mailerService.SendCreateEventConfirmationMail(newEvent);
    }

    public async Task HandleUpdateEventMails(
        Event updatedEvent,
        IEnumerable<Participant> participantsToAdd,
        IEnumerable<Participant> participantsToDelete
    )
    {
        var userIds = participantsToAdd
                .Select(p => p.UserId)
                .ToHashSet();

        var userIdsToSendRevokeMailTo = participantsToDelete
            .Where(p => !userIds.Contains(p.UserId))
            .Select(p => p.UserId);

        await mailerService.SendRevokeInvitationMail(
           updatedEvent, userIdsToSendRevokeMailTo);

        if (userIds.Count != 0)
        {
            await mailerService.SendInvitationUpdateMail(updatedEvent, userIds);
        }

        await mailerService.SendUpdateEventConfirmationMail(updatedEvent);
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

        await mailerService.SendEventCanceledMail(
            eventToDelete, eventParticipants);

        await mailerService.SendDeleteEventConfirmationMail(eventToDelete, ownerId);
    }
}
