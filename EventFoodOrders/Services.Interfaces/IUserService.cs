using EventFoodOrders.Dto.UserDTOs;
using EventFoodOrders.Utilities;

namespace EventFoodOrders.Services.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Returns a list of UserDtos whose names start with the queryString.
    /// </summary>
    /// <param name="queryString"></param>
    /// <returns></returns>
    //Task<List<UserDto>> GetUsersFromQuery(string queryString);


    /// <summary>
    /// Returns a list of UserDtos whose names start with the queryString.
    /// </summary>
    /// <param name="queryString"></param>
    /// <param name="eventId"></param>
    /// <returns></returns>
    Task<List<UserDto>> GetUsersFromQuery(string queryString, Guid? eventId);


    /// <summary>
    /// Gets a list of names of users from their ids.
    /// </summary>
    /// <param name="userIds"></param>
    /// <returns></returns>

    Task<List<string>> GetNamesWithIds(List<Guid> userIds);

    /// <summary>
    /// Sends an email to a list of users.
    /// </summary>
    /// <param name="userIds"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    Task SendEmail(List<Guid> userIds, EmailTemplate message);

    /// <summary>
    /// Gets a list of UserDtos from a Guid array of user Ids.
    /// </summary>
    /// <param name="userIds"></param>
    /// <returns></returns>
    Task<List<UserDto>> GetUsersFromIds(Guid[] userIds);

    /// <summary>
    /// Returns a user dto using their id.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<UserDto?> GetUserWithId(Guid userId);

    /// <summary>
    /// Returns a list of ids for the users in a group from the group id.
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    Task<List<Guid>> GetUsersFromGroup(Guid groupId);
}
