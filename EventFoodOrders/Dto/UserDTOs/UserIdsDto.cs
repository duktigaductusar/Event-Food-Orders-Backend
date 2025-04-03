using Newtonsoft.Json;

namespace EventFoodOrders.Dto.UserDTOs
{
    public record UserIdsDto
    {
        [JsonProperty]
        public required Guid[] UserIds { get; init; }
    }
}
