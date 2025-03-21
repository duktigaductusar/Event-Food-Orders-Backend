using System.IdentityModel.Tokens.Jwt;
using EventFoodOrders.security;
using Newtonsoft.Json;
using IAuthorizationService = EventFoodOrders.Interfaces.IAuthorizationService;

namespace EventFoodOrders.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly HttpClient _httpClient;
    private readonly string _tenantId;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _redirectUri;

    public AuthorizationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _tenantId = Environment.GetEnvironmentVariable("AzureAd__TenantId") ?? throw new ArgumentNullException("TENANT_ID not set in environment variable");
        _clientId = Environment.GetEnvironmentVariable("AzureAd__ClientId") ?? throw new ArgumentNullException("CLIENT_ID not set in environment variable");
        _clientSecret = Environment.GetEnvironmentVariable("AzureAd__ClientSecret") ?? throw new ArgumentNullException("CLIENT_SECRET not set in environment variable");
        _redirectUri = Environment.GetEnvironmentVariable("AzureAd__RedirectUri") ?? throw new ArgumentNullException("REDIRECT_URI not set in environment variable");
    }

    public string GetLoginUrl()
    {
        var scope = "openid profile email https://graph.microsoft.com/Mail.Send";
        
        return $"https://login.microsoftonline.com/{_tenantId}/oauth2/v2.0/authorize?" + $"client_id={_clientId}&response_type=code&redirect_uri={_redirectUri}&scope={scope}&response_mode=query";
    }

    public async Task<AuthResponse> ExchangeCodeForTokenAsync(string code)
    {

        var formData = new FormUrlEncodedContent([
            new KeyValuePair<string, string>("client_id", _clientId),
            new KeyValuePair<string, string>("client_secret", _clientSecret),
            new KeyValuePair<string, string>("redirect_uri", _redirectUri),
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            new KeyValuePair<string, string>("code", code),
            new KeyValuePair<string, string>("scope", "openid profile email https://graph.microsoft.com/.default")
        ]);

        var response = await _httpClient.PostAsync(
            $"https://login.microsoftonline.com/{_tenantId}/oauth2/v2.0/token", formData);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to exchange code for token.");
        }
        
        var content = await response.Content.ReadAsStringAsync();
        var authResponse = JsonConvert.DeserializeObject<AuthResponse>(content);
        if (authResponse == null)
        {
            throw new NullReferenceException("JSON Deserialization return null.");
        }

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(authResponse.idToken);
        
        authResponse.UserId = jwt.Claims.FirstOrDefault(c => c.Type == "oid")?.Value
                              ?? jwt.Claims.FirstOrDefault(c => c.Type == "sub")?.Value!;
        authResponse.Email = jwt.Claims.FirstOrDefault(c => c.Type == "email")?.Value
                             ?? jwt.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value!;
        authResponse.DisplayName = jwt.Claims.FirstOrDefault(c => c.Type == "name")?.Value!;

        if (authResponse.UserId == null || authResponse.Email == null || authResponse.DisplayName == null)
        {
            throw new NullReferenceException("UserId, Email or DisplayName returned null when deserializing token.");
        }
        
        return authResponse;
    }
}