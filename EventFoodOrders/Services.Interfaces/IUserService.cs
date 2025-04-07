using EventFoodOrders.Dto.UserDTOs;

namespace EventFoodOrders.Services.Interfaces;

public interface IUserService
{
    Task<List<UserDto>> GetUsersFromQuery(string queryString, Guid? eventId);
    Task<List<string>> GetNamesWithIds(List<Guid> userIds);
    Task SendEmail(List<Guid> userIds, string message);
    Task<List<UserDto>> GetUsersFromIds(Guid[] userIds);
}
