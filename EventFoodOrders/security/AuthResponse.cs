//2025-03 RR

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
    
    [JsonProperty("id_token")]
    public string idToken { get; set; }
    
    public string UserId { get; set; }
    public string Email { get; set; }
    
    //Not actually used atm
    public string DisplayName { get; set; }
}