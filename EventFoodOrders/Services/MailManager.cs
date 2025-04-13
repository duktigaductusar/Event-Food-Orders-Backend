using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Entities;
using EventFoodOrders.Services.Interfaces;

namespace EventFoodOrders.Services;

public class MailManager(IMailerService mailerService) : IMailManager
{
    public async Task HandleNewEventMails(
        EventForCreationDto newEvent,
        Guid ownerId,
        Guid eventId
    )
    {
        if (newEvent.UserIds?.Length != null && newEvent.UserIds?.Length > 0)
        {
            await mailerService.SendInvitationMail(newEvent, ownerId, eventId);
        }
        await mailerService.SendCreateEventConfirmationMail(newEvent, ownerId, eventId);
    }

    public async Task HandleUpdateEventMails(
        EventForUpdateDto updatedEventDto,
        Event updatedEvent,
        IEnumerable<Participant> participantsToAdd,
        IEnumerable<Participant> participantsToDelete,
        Guid ownerId,
        Guid eventId
    )
    {
        var focusedEvent = updatedEventDto with
        {
            UserIds = participantsToAdd
                .Select(p => p.UserId)
                .ToArray()
        };

        var participantsToAdIds = participantsToAdd.Select(p => p.UserId).ToHashSet();

        var participantsToSendRevokeMailTo = participantsToDelete.Where(p => !participantsToAdIds.Contains(p.UserId));

        await mailerService.SendRevokeInvitationMail(
           updatedEvent, participantsToSendRevokeMailTo.Select(p => p.UserId));

        if (focusedEvent.UserIds?.Length != null && focusedEvent.UserIds?.Length > 0)
        {
            await mailerService.SendInvitationMail(focusedEvent, ownerId, eventId);
        }
        await mailerService.SendUpdateEventConfirmationMail(focusedEvent, ownerId, eventId);
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
