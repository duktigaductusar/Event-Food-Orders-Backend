using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Entities;

namespace EventFoodOrders.Services;

public interface IMailerService
{
    /// <summary>
    /// Sends confirmation mail for new event to creator.
    /// </summary>
    /// <param name="focusedEvent"></param>
    /// <returns></returns>
    Task SendCreateEventConfirmationMail(Event focusedEvent);

    /// <summary>
    /// Sends confirmation mail for updated event to creator.
    /// </summary>
    /// <param name="focusedEvent"></param>
    /// <returns></returns>
    Task SendUpdateEventConfirmationMail(Event focusedEvent);

    /// <summary>
    /// Send delete confirmation mail to the user that deleted the event.
    /// </summary>
    /// <param name="focusedEvent"></param>
    /// <param name="ownerId"></param>
    /// <returns></returns>
    Task SendDeleteEventConfirmationMail(Event focusedEvent, Guid ownerId);

    /// <summary>
    /// Send event canceled event to participants in the event.
    /// </summary>
    /// <param name="focusedEvent"></param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    Task SendEventCanceledMail(Event focusedEvent, IEnumerable<Guid> userIds);

    /// <summary>
    /// Sends the initial invitation email to all invited participants.
    /// </summary>
    /// <param name="focusedEvent"></param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    Task SendInvitationMail(Event focusedEvent, IEnumerable<Guid> userIds);

    /// <summary>
    /// Sends the invitation email to newly invited participants.
    /// </summary>
    /// <param name="focusedEvent"></param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    Task SendInvitationUpdateMail(Event focusedEvent, IEnumerable<Guid> userIds);
    
    /// <summary>
    /// Sends reminder emails at a predetermined time on the deadline day.
    /// </summary>
    /// <param name="recipients"></param>
    /// <param name="focusedEvent"></param>
    /// <param name="eventId"></param>
    /// <returns></returns>
    Task SendReminderMail(List<Guid> recipients, Event focusedEvent, Guid eventId);

    /// <summary>
    /// Sends a summary email to the event owner. Needs an Event entity as a parameter.
    /// </summary>
    /// <param name="focusedEvent"></param>
    /// <returns></returns>
    Task SendSummaryMail(Event focusedEvent);
    
    /// <summary>
    /// Send revoke invitation mail to deleted participants.
    /// </summary>
    /// <param name="focusedEvent"></param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    Task SendRevokeInvitationMail(Event focusedEvent, IEnumerable<Guid> userIds);
}