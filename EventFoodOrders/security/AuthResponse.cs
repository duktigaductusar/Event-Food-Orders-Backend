using Newtonsoft.Json;

namespace EventFoodOrders.security;

public class AuthResponse
{
    [JsonProperty("access_token")]
    public string accessToken { get; set; }
    
    [JsonProperty("expires_in")]
    public int expiresIn { get; set; }
    
    [JsonProperty("token_type")]
    public string tokenType { get; set; }
}