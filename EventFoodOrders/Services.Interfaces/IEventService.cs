using EventFoodOrders.Dto.EventDTOs;

namespace EventFoodOrders.Services.Interfaces;

// Handles getting, creating, and updating events using both event and participant repositories.
public interface IEventService
{
    // Creates an event using a Dto with information. Also creates an owner participant and any other invited participants.
    EventForResponseDto CreateEvent(Guid userId, EventForCreationDto eventForCreation);

    // Deletes an event.
    bool DeleteEvent(Guid eventId);

    // Gets all the events that a user has been invited to.
    IEnumerable<EventForResponseDto> GetAllEventsForUser(Guid userId);

    // Gets a single event, given that the user has been invited to it.
    EventForResponseWithDetailsDto GetEventForUser(Guid userId, Guid eventId);

    // Updates an event given its Id and a Dto.
    EventForResponseDto UpdateEvent(Guid eventId, EventForUpdateDto updatedEventDto);
}