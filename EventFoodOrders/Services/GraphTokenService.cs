using Microsoft.Identity.Client;

namespace EventFoodOrders.Services;

public class GraphTokenService : IGraphTokenService
{
    private readonly IConfidentialClientApplication _confidentialClient;
    private readonly string[] _scopes;

    public GraphTokenService(IConfiguration config)
    {
        _confidentialClient = ConfidentialClientApplicationBuilder.Create(config["AzureAd:ClientId"])
            .WithClientSecret(config["AzureAd:ClientSecret"])
            .WithAuthority(new Uri($"https://login.microsoft.com/{config["AzureAd:TenantId"]}"))
            .Build();
        _scopes = ["https://graph.microsoft.com/.default"];
    }

    public async Task<string> GetAccessToken()
    {
        var result = await _confidentialClient.AcquireTokenForClient(_scopes).ExecuteAsync();
        return result.AccessToken;
    }
}