using EventFoodOrders.Exceptions;
using EventFoodOrders.Services.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EventFoodOrders.Mock
{
    public class MockUserService(IUserSeed seeder) : IUserService
    {
        readonly List<MockUser> users = seeder.Users;

        public string GetNameWithId(Guid userId)
        {
            MockUser? user = users.FirstOrDefault(u => u.UserId == userId);
            if (user is null)
            {
                throw new CustomException(StatusCodes.Status500InternalServerError, "User not found.");
            }
            return user.Username;
        }

        public List<string> GetNamesWithIds(List<Guid> userIds)
        {
            List<string> userNames = [];
            foreach (var userId in userIds)
            {
                try
                {
                    string name = GetNameWithId(userId);
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

        public void SendEmail(List<Guid> userIds, string message)
        {
            foreach (var userId in userIds)
            {
                Console.WriteLine($"Sending an email to... {GetNameWithId(userId)}");
            }
        }

        public List<string> GetEmailAddresses(string queryString)
        {
            List<string> filteredEmails = [.. users
                .Where(u => u.Username.StartsWith(queryString, StringComparison.OrdinalIgnoreCase) ||
                    (u.Email != null && u.Email.StartsWith(queryString, StringComparison.OrdinalIgnoreCase)))
                .Select(u => u.Email)];

            return filteredEmails;
        }
    }
}
