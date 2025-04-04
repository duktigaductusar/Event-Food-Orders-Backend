using EventFoodOrders.Dto.UserDTOs;

namespace EventFoodOrders.Services.Interfaces;

public interface IUserService
{
    Task <List<UserDto>> GetUsersFromQuery(string queryString);
    List<string> GetNamesWithIds(List<Guid> userIds);
    Task SendEmail(List<Guid> userIds);
    Task <List<UserDto>> GetUsersFromIds(Guid[] userIds);
}
