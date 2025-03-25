using EventFoodOrders.Dto.EventDTOs;

namespace EventFoodOrders.Services.Interfaces;

public interface IEventService
{
    EventForResponseDto CreateEvent(Guid userId, EventForCreationDto eventForCreation);
    bool DeleteEvent(Guid eventId);
    IEnumerable<EventForResponseDto> GetAllEventsForUser(Guid userId);
    EventForResponseWithDetailsDto GetEventForUser(Guid userId, Guid eventId);
    EventForResponseDto UpdateEvent(Guid eventId, EventForUpdateDto updatedEventDto);
}