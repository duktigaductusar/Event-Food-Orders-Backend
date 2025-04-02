using EventFoodOrders.Dto.UserDTOs;

namespace EventFoodOrders.Services.Interfaces;

public interface IUserService
{
    Task<UserDto[]> GetUsersFromQuery(string queryString);
    Task<UserDto> GetUserWithId(Guid userId);
    List<string> GetNamesWithIds(Guid[] userIds);
    Task SendEmail(Guid[] userIds);
    Task<UserDto[]> GetUsersFromIds(Guid[] userIds);
}
