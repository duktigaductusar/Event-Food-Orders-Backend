using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Entities;

namespace EventFoodOrders.Services.Interfaces
{
    public interface IMailManager
    {
        Task HandleCancelEventMails(Event eventToDelete, Guid ownerId);
        Task HandleNewEventMails(EventForCreationDto newEvent, Guid ownerId, Guid eventId);
        Task HandleUpdateEventMails(EventForUpdateDto updatedEventDto, Event updatedEvent, IEnumerable<Participant> participantsToAdd, IEnumerable<Participant> participantsToDelete, Guid ownerId, Guid eventId);
    }
}