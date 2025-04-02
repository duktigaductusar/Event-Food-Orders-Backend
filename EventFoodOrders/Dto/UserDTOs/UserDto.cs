using Newtonsoft.Json;

namespace EventFoodOrders.Dto.UserDTOs
{
    public record UserDto
    {
        [JsonProperty("id")]
        public Guid UserId { get; set; }
        [JsonProperty("displayName")]
        public string Username { get; set; }
        [JsonProperty("mail")]
        public string Email { get; set; }
    }
}
