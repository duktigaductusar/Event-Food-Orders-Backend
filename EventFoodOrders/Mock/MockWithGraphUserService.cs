using System.Collections.Generic;
using EventFoodOrders.Dto.UserDTOs;
using EventFoodOrders.Repositories.Interfaces;
using EventFoodOrders.Services;
using EventFoodOrders.Services.Interfaces;

namespace EventFoodOrders.Mock
{
    public class MockWithGraphUserService(
        IGraphTokenService graphTokenService,
        HttpClient httpClient,
        IConfiguration config,
        IUserSeed seeder,
        IUoW uow
    ) : IUserService
    {
        private IUserService _userService = new UserService(
            graphTokenService, httpClient, config, uow);
        private IUserService _mockService = new MockUserService(seeder);

        public async Task<List<string>> GetNamesWithIds(List<Guid> userIds)
        {
            return await _userService.GetNamesWithIds(userIds);
        }

        public async Task<List<UserDto>> GetUsersFromIds(Guid[] userIds)
        {
            List<UserDto> mockUsers = await _mockService.GetUsersFromIds(userIds);
            List<Guid> remainingUserIds = GetUniqueUserIdsFromIds(mockUsers, [.. userIds]);
            List<UserDto> graphUsers = await _userService.GetUsersFromIds(remainingUserIds.ToArray());
            List<UserDto> uniqueUsers = GetUniqueUserList(mockUsers, graphUsers);

            return uniqueUsers;
        }

        public async Task<List<UserDto>> GetUsersFromQuery(string queryString, Guid? eventId)
        {
            List<UserDto> mockUsers = await _mockService.GetUsersFromQuery(queryString, eventId);
            List<UserDto> graphUsers = await _userService.GetUsersFromQuery(queryString, eventId);
            List<UserDto> uniqueUsers = GetUniqueUserList(mockUsers, graphUsers);

            return uniqueUsers;
        }

        public async Task SendEmail(List<Guid> userIds, string message)
        {
            await _userService.SendEmail(userIds, message);
        }

        private static List<UserDto> GetUniqueUserList(List<UserDto> firstList, List<UserDto> secondList)
        {
            List<UserDto> uniqueUsers = [.. firstList
                .Concat(secondList)
                .GroupBy(u => u.UserId)
                .Select(group => group.First())];

            uniqueUsers.Sort((i, p) => i.Email.CompareTo(p.Email));

            return uniqueUsers;
        }

        private static List<Guid> GetUniqueUserIdsFromIds(List<UserDto> firstList, List<Guid> ids)
        {
            var userIdsSet = new HashSet<Guid>(firstList.Select(u => u.UserId));
            var uniqueIds = ids.Where(guid => !userIdsSet.Contains(guid)).ToList();

            return uniqueIds;
        }
    }
}
