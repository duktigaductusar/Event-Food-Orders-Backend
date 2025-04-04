using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Dto.ParticipantDTOs;
using EventFoodOrders.Dto.UserDTOs;

namespace EventFoodOrders.Services.Interfaces;

/// <summary>
/// Handles getting, creating, and updating events using both event and participant repositories.
/// </summary>
public interface IEventService
{
    /// <summary>
    /// Creates an event using a Dto with information. Also creates an owner participant and any other invited participants.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="eventForCreation"></param>
    /// <returns></returns>
    EventForResponseDto CreateEvent(Guid userId, EventForCreationDto eventForCreation);

    /// <summary>
    /// Deletes an event.
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns></returns>
    bool DeleteEvent(Guid eventId);

    /// <summary>
    /// Gets all the events that a user has been invited to.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    IEnumerable<EventForResponseDto> GetAllEventsForUser(Guid userId);

    /// <summary>
    /// Gets a single event, given that the user has been invited to it.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="eventId"></param>
    /// <returns></returns>
    EventForResponseWithDetailsDto GetEventForUser(Guid userId, Guid eventId);

    /// <summary>
    /// Gets a single event with all the participant and user information.
    /// </summary>
    /// <returns></returns>
    EventForResponseWithUsersDto GetEventWithUsers(EventForResponseWithDetailsDto eventDto, IEnumerable<ParticipantForResponseDto> participantDtos, IEnumerable<UserDto> users);

    /// <summary>
    /// Updates an event given its Id and a Dto.
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="updatedEventDto"></param>
    /// <returns></returns>
    EventForResponseDto UpdateEvent(Guid eventId, EventForUpdateDto updatedEventDto);
}