using Newtonsoft.Json;

namespace EventFoodOrders.Mock;

public class UserSeed : IUserSeed
{
    public List<MockUser> Users { get; set; }

    public UserSeed()
    {
        string filePath = "Mock/UserSeed.json"; // You may need to adjust the path if needed
        string json = File.ReadAllText(filePath);

        // Deserialize the JSON into a list of User objects
        Users = JsonConvert.DeserializeObject<List<MockUser>>(json)!;
    }
}
