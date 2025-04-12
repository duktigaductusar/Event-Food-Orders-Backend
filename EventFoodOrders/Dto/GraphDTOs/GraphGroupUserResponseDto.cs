using Newtonsoft.Json;

namespace EventFoodOrders.Dto.GraphDTOs;

public class GraphGroupUserResponseDto
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    [JsonProperty("mail")]
    public string? Mail { get; set; }
}