using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text;
using EventFoodOrders.Dto.UserDTOs;
using EventFoodOrders.Services.Interfaces;
using Newtonsoft.Json;

namespace EventFoodOrders.Services;

public class UserService : IUserService
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
    public async Task<List<UserDto>> GetUsersFromQuery(string queryString, Guid? eventId)
    {
        await SetAccessToken();
        var encodedSearchString = Uri.EscapeDataString(queryString);
        var queryGroup = $"groups?$filter=startswith(displayName, '{encodedSearchString}')";
        var groupResponse = await _httpClient.GetAsync(queryGroup);
        //groupResponse.EnsureSuccessStatusCode();
        var queryUser = $"users?$filter=startswith(displayName,'{encodedSearchString}')";
        var userResponse = await _httpClient.GetAsync(queryUser);
        //userResponse.EnsureSuccessStatusCode();
        
        var userContent = await userResponse.Content.ReadAsStringAsync();
        var groupContent = await groupResponse.Content.ReadAsStringAsync();
        List<UserDto> result = [];
        var groupResult = JsonConvert.DeserializeObject<GraphUsersResponse>(groupContent)!.Value;
        var userResult= JsonConvert.DeserializeObject<GraphUsersResponse>(userContent)!.Value;
        result.AddRange(groupResult);
        result.AddRange(userResult);
        return result;
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

    public async Task<List<string>> GetNamesWithIds(List<Guid> userIds)
    {
        throw new NotImplementedException();
    }

    public async Task SendEmail(List<Guid> userIds, string message = "")
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

    public async Task <List<UserDto>> GetUsersFromIds(Guid[] userIds)
    {
        Collection<UserDto> users = [];
        foreach (Guid id in userIds)
        {
            var user = await GetUserWithId(id);
            users.Add(user);
        }
        return users.ToList();
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