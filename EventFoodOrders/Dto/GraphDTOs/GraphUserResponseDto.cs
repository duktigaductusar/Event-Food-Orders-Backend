using EventFoodOrders.Dto.UserDTOs;
using Newtonsoft.Json;

namespace EventFoodOrders.Dto.GraphDTOs;

public class GraphUsersResponseDto
{
    [JsonProperty("value")]
    public UserDto[] Users { get; set; }
}