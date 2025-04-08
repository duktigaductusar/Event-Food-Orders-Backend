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
    Task<List<UserDto>> GetUsersFromQuery(string queryString);

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
    Task SendEmail(List<Guid> userIds, string message);

    /// <summary>
    /// Gets a list of UserDtos from a Guid array of user Ids.
    /// </summary>
    /// <param name="userIds"></param>
    /// <returns></returns>
    Task<List<UserDto>> GetUsersFromIds(Guid[] userIds);
}
