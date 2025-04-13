using EventFoodOrders.Dto.UserDTOs;
using EventFoodOrders.Exceptions;
using EventFoodOrders.Repositories.Interfaces;
using EventFoodOrders.Services.Interfaces;
using EventFoodOrders.Utilities;
using Sprache;

namespace EventFoodOrders.Mock;

public class MockUserService(IUserSeed seeder, IUoW uow) : IUserService
{
    readonly List<MockUser> users = seeder.Users;
    private readonly IUoW _uow = uow;

    public async Task<string> GetNameWithId(Guid userId)
    {
        MockUser? user = users.FirstOrDefault(u => u.UserId == userId);
        if (user is null)
        {
            throw new CustomException(StatusCodes.Status500InternalServerError, "User not found.");
        }
        return user.Username;
    }

    public async Task<List<string>> GetNamesWithIds(List<Guid> userIds)
    {
        List<string> userNames = [];
        foreach (var userId in userIds)
        {
            try
            {
                string name = await GetNameWithId(userId);
                if (name is not null)
                {
                    userNames.Add(name);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        return userNames;
    }

    public async Task SendEmail(List<Guid> userIds, EmailTemplate message)
    {
        foreach (var userId in userIds)
        {
            Console.WriteLine($"Sending an email to... {GetNameWithId(userId)}");
        }
    }

    public async Task<List<UserDto>> GetUsersFromQuery(string queryString, Guid? eventId)
    {
        List<MockUser> filteredUsers = [.. users
            .Where(u => u.Username.StartsWith(queryString, StringComparison.OrdinalIgnoreCase) ||
                (u.Email != null && u.Email.StartsWith(queryString, StringComparison.OrdinalIgnoreCase)))];

        List<UserDto> dtos = [];

        foreach (var user in filteredUsers)
        {
            dtos.Add(new UserDto
            {
                Username = user.Username,
                Email = user.Email,
                UserId = user.UserId
            });
        }

        if (eventId == null) { return dtos; }

        var participantsForEvent = await _uow.EventRepository.GetParticipantsByEventId(eventId.Value);
        var participantIdsForEvent = participantsForEvent.Select(p => p.UserId).ToHashSet();
        return [.. dtos.Where(u => !participantIdsForEvent.Contains(u.UserId))];
    }

    public async Task<List<UserDto>> GetUsersFromIds(Guid[] userIds)
    {
        List<MockUser> filteredUsers = [.. users
            .Where(u => userIds.Contains(u.UserId)).ToList()];

        List<UserDto> dtos = [];

        foreach (var user in filteredUsers)
        {
            dtos.Add(new UserDto
            {
                Username = user.Username,
                Email = user.Email,
                UserId = user.UserId
            });
        }

        return dtos;
    }

    public async Task<UserDto?> GetUserWithId(Guid userId)
    {
        MockUser? user = users.Where(u => u.UserId == userId).FirstOrDefault();
        if (user is not null)
        {
            return new UserDto()
            {
                UserId = userId,
                Username = user.Username,
                Email = user.Email
            };
        }
        return null;
    }

    public async Task<List<Guid>> GetUsersFromGroup(Guid groupId)
    {
        return [];
    }
}
