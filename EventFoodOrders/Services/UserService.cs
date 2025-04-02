using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text;
using EventFoodOrders.Dto.UserDTOs;
using EventFoodOrders.Services.Interfaces;
using Newtonsoft.Json;

namespace EventFoodOrders.Services;

public class UserService: IUserService
{
    private readonly IGraphTokenService _graphTokenService;
    private readonly HttpClient _httpClient;
    private string _accessToken;
    private IConfiguration _config;
    
    public UserService(IGraphTokenService graphTokenService, HttpClient httpClient, IConfiguration config)
    {
        _graphTokenService = graphTokenService;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://graph.microsoft.com/v1.0/");
        _config = config;
    }
    public async Task<UserDto[]> GetUsersFromQuery(string queryString)
    {
        await SetAccessToken();
        var encodedSearchString = Uri.EscapeDataString(queryString);
        var query = $"users?$filter=startswith(displayName,'{encodedSearchString}')";
        var response = await _httpClient.GetAsync(query);
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<GraphUsersResponse>(content)!;
        return result.Value ?? [];
    }

    public async Task<UserDto> GetUserWithId(Guid userId)
    {
        await SetAccessToken();
        var searchId = userId.ToString();
        var response = await _httpClient.GetAsync($"users/{searchId}");
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<UserDto>(content)!;
    }

    public List<string> GetNamesWithIds(Guid[] userIds)
    {
        throw new NotImplementedException();
    }

    public async Task SendEmail(Guid[] userIds)
    {
        Console.WriteLine("Email sending method starting");
        Collection<string> recipients = [];
        foreach (var userId in userIds)
        {
            var recipient = await GetUserWithId(userId);
            var recipientEmail = recipient.Email;
            recipients.Add(recipientEmail);
        }
        Console.WriteLine("Finished fetching recipients");

        await SetAccessToken();
        var mailPayload = new
        {
            message = new
            {
                subject = "Test Email from Matbeställningar",
                body = new
                {
                    contentType = "Text",
                    content = "This is a test mail that Matbeställningar sent using its backend Graph API call"
                },
                toRecipients = recipients.Select(email => new { emailAddress = new { address = email } }).ToArray()
            },
            saveToSentItems = false
        };
        Console.WriteLine("Finished building email template and recipients");
        
        var jsonPayload = JsonConvert.SerializeObject(mailPayload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var requestUri = $"users/{_config["Graph:SenderEmail"]}/sendMail";
        Console.WriteLine("Emails have been sent, returning response status code");
        var response = await _httpClient.PostAsync(requestUri, content);
        response.EnsureSuccessStatusCode();
    }

    public async Task <UserDto[]> GetUsersFromIds(Guid[] userIds)
    {
        throw new NotImplementedException();
    }
    
    private async Task SetAccessToken()
    {
        if (string.IsNullOrEmpty(_accessToken))
        {
            _accessToken = await _graphTokenService.GetAccessToken();
        }
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
    }
    
    //For testing purposes
    public class GraphUsersResponse
    {
        [JsonProperty("value")]
        public UserDto[] Value { get; set; }
    }
}