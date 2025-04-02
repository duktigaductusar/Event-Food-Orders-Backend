using EventFoodOrders.Dto.UserDTOs;

namespace EventFoodOrders.Repositories.Interfaces;

public interface IGraphRepository
{
    Task<UserDto> GetUserAsync(Guid userId);
    Task SendMailAsync(Guid[] userIds);
}