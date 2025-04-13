using EventFoodOrders.Dto.UserDTOs;
using EventFoodOrders.Exceptions;
using EventFoodOrders.Repositories.Interfaces;
using EventFoodOrders.Services.Interfaces;
using EventFoodOrders.Utilities;
using Sprache;
using System.Text;

namespace EventFoodOrders.Mock;

public class MockUserService(IUserSeed seeder, IUoW uow) : IUserService
{
    readonly List<MockUser> users = seeder.Users;
    private readonly IUoW _uow = uow;
    private readonly string _mockEmailRootFolder = Path.Combine(Directory.GetCurrentDirectory(), "MockMails");

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
            var userFolderPath = Path.Combine(_mockEmailRootFolder, $"UserId__{userId}");
            Directory.CreateDirectory(userFolderPath);

            var fileName = MakeSafeFileName(GetEmailTitleWithTimeStamp(message)) + ".txt";
            var filePath = Path.Combine(userFolderPath, fileName);

            var content = new StringBuilder()
                .AppendLine($"To: {userId}")
                .AppendLine($"Date: {DateTime.Now}")
                .AppendLine($"Subject: {GetEmailTitle(message)}")
                .AppendLine()
                .AppendLine(GetEmailContent(message))
                .ToString();

            await File.WriteAllTextAsync(filePath, content);
            Console.WriteLine($"Mock email written to: {filePath}");
        }
    }

    private static string GetEmailTitle(EmailTemplate template) => 
       template.Subject  ?? "NoTitle";

    private static string GetEmailTitleWithTimeStamp(EmailTemplate template)
    {
        var baseTitle = GetEmailTitle(template);
        var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
        return $"{baseTitle}_{timestamp}";
    }

    private static string GetEmailContent(EmailTemplate template) =>
        template.Body ?? "NoContent";

    private static string MakeSafeFileName(string name)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '_');
        }
        return name;
    }

    public async Task<List<UserDto>> GetUsersFromQuery(string queryString, Guid? eventId)
    {
        List<MockUser> filteredUsers = users
            .Where(u => u.Username.StartsWith(queryString, StringComparison.OrdinalIgnoreCase) ||
                (u.Email != null && u.Email.StartsWith(queryString, StringComparison.OrdinalIgnoreCase)))
            .ToList();

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
        
        var participantIdsForEvent = participantsForEvent
            .Select(p => p.UserId)
            .ToHashSet();
        
        return dtos
            .Where(u => !participantIdsForEvent.Contains(u.UserId))
            .ToList();
    }

    public async Task<List<UserDto>> GetUsersFromIds(Guid[] userIds)
    {
        List<MockUser> filteredUsers = users
            .Where(u => userIds.Contains(u.UserId))
            .ToList();

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
        MockUser? user = users
            .Where(u => u.UserId == userId)
            .FirstOrDefault();
        
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
