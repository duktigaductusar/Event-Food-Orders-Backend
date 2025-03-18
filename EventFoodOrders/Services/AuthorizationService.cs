using System;
using System.Net.Http;
using System.Threading.Tasks;
using EventFoodOrders.security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using IAuthorizationService = EventFoodOrders.Interfaces.IAuthorizationService;

namespace EventFoodOrders.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public AuthorizationService(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public string GetLoginUrl()
    {
        string tenantId = _configuration["AzureAd:TenantId"]!;
        string clientId = _configuration["AzureAd:ClientId"]!;
        string redirectUri = _configuration["AzureAd:RedirectUri"]!;
        var scope = "openid profile email https://graph.microsoft.com/Mail.Send";
        
        return $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/authorize?" + $"client_id={clientId}&response_type=code&redirect_uri={redirectUri}&scope={scope}&response_mode=query";
    }

    public async Task<AuthResponse> ExchangeCodeForTokenAsync(string code)
    {
        var tenantId = _configuration["AzureAd:TenantId"];
        var clientId = _configuration["AzureAd:ClientId"];
        var clientSecret = _configuration["AzureAd:ClientSecret"];
        var redirectUri = _configuration["AzureAd:RedirectUri"];

        var formData = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("client_secret", clientSecret),
            new KeyValuePair<string, string>("redirect_uri", redirectUri),
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            new KeyValuePair<string, string>("code", code),
            new KeyValuePair<string, string>("scope", "https://graph.microsoft.com/.default"),
        });

        var response = await _httpClient.PostAsync(
            $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token", formData);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to exchange code for token.");
        }
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AuthResponse>(content);
    }
}