using EventFoodOrders.Dto.UserDTOs;
using EventFoodOrders.Utilities;

namespace EventFoodOrders.Services.Interfaces;

public interface IUserService
{
    Task<List<UserDto>> GetUsersFromQuery(string queryString);
    Task<List<string>> GetNamesWithIds(List<Guid> userIds);
    Task SendEmail(List<Guid> userIds, EmailTemplate message);
    Task<List<UserDto>> GetUsersFromIds(Guid[] userIds);
}
