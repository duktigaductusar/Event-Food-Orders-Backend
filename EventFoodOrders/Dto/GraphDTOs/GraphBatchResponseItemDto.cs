using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace EventFoodOrders.Dto.GraphDTOs;

public class GraphBatchResponseItemDto
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("status")]
    public int Status { get; set; }

    [JsonProperty("body")]
    public JObject Body { get; set; } = [];
}
