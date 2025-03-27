using EventFoodOrders.Exceptions;
using Newtonsoft.Json;

namespace EventFoodOrders.Services
{
    public class UserService
    {
        readonly List<User> users;

        public UserService()
        {
            string filePath = "Seed/users.json"; // You may need to adjust the path if needed
            string json = File.ReadAllText(filePath);

            // Deserialize the JSON into a list of User objects
            users = JsonConvert.DeserializeObject<List<User>>(json);
        }

        public string GetName(Guid userId)
        {
            User? user = users.FirstOrDefault(u => u.UserId == userId);
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
            foreach(var userId in userIds)
            {
                Console.WriteLine($"Sending an email to... {GetName(userId)}");
            }
        }
    }

    public class User
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
