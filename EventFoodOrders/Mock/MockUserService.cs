using EventFoodOrders.Exceptions;
using EventFoodOrders.Services.Interfaces;

namespace EventFoodOrders.Mock
{
    public class MockUserService(IUserSeed seeder) : IUserService
    {
        readonly List<MockUser> users = seeder.Users;

        public string GetName(Guid userId)
        {
            MockUser? user = users.FirstOrDefault(u => u.UserId == userId);
            if (user is null)
            {
                throw new CustomException(StatusCodes.Status500InternalServerError, "User not found.");
            }
            return user.Username;
        }

        public List<string> GetNames(List<Guid> userIds)
        {
            List<string> userNames = [];
            foreach (var userId in userIds)
            {
                try
                {
                    string name = GetName(userId);
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
                Console.WriteLine($"Sending an email to... {GetName(userId)}");
            }
        }
    }
}
