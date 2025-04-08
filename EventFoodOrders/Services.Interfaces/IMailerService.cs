using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Entities;

namespace EventFoodOrders.Services;

public interface IMailerService
{
    /// <summary>
    /// Sends the initial invitation email to all invited participants.
    /// </summary>
    /// <param name="focusedEvent"></param>
    /// <param name="ownerId"></param>
    /// <param name="eventId"></param>
    /// <returns></returns>
    Task SendInvitationMail(EventForCreationDto focusedEvent, Guid ownerId, Guid eventId);
    
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
}