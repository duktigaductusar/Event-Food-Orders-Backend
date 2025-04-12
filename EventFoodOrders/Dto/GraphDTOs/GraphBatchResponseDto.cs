using Newtonsoft.Json;

namespace EventFoodOrders.Dto.GraphDTOs;

public class GraphBatchResponseDto
{
    [JsonProperty("responses")]
    public List<GraphBatchResponseItemDto> Responses { get; set; } = [];
}
