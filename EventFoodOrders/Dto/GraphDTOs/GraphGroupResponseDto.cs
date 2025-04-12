using Newtonsoft.Json;

namespace EventFoodOrders.Dto.GraphDTOs;

public class GraphGroupResponseDto
{
    [JsonProperty("value")]
    public GraphGroupUserResponseDto[] Members { get; set; }
}
