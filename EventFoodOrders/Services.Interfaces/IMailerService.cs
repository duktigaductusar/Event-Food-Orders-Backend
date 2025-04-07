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
    /// <returns></returns>
    Task SendInvitationMail(EventForCreationDto focusedEvent, Guid ownerId);
    
    /// <summary>
    /// Sends reminder emails at a predetermined time on the deadline day.
    /// </summary>
    /// <returns></returns>
    Task SendReminderMail(List<Guid> recipients, Event focusedEvent);
}