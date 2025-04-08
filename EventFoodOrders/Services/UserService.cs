using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text;
using EventFoodOrders.Dto.UserDTOs;
using EventFoodOrders.Repositories.Interfaces;
using EventFoodOrders.Services.Interfaces;
using EventFoodOrders.Utilities;
using Newtonsoft.Json;

namespace EventFoodOrders.Services;

public class UserService : IUserService
{
    private readonly IGraphTokenService _graphTokenService;
    private readonly HttpClient _httpClient;
    private string _accessToken;
    private IConfiguration _config;
    private IUoW _uow;
    
    public UserService(
        IGraphTokenService graphTokenService,
        HttpClient httpClient,
        IConfiguration config,
        IUoW uow
        )
    {
        _graphTokenService = graphTokenService;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://graph.microsoft.com/v1.0/");
        _config = config;
        _uow = uow;
    }
    public async Task<List<UserDto>> GetUsersFromQuery(string queryString, Guid? eventId)
    {
        await SetAccessToken();
        var encodedSearchString = Uri.EscapeDataString(queryString);
        var queryGroup = $"groups?$filter=startswith(displayName, '{encodedSearchString}')";
        var groupResponse = await _httpClient.GetAsync(queryGroup);
        groupResponse.EnsureSuccessStatusCode();
        var queryUser = $"users?$filter=startswith(displayName,'{encodedSearchString}')";
        var userResponse = await _httpClient.GetAsync(queryUser);
        userResponse.EnsureSuccessStatusCode();
        
        var userContent = await userResponse.Content.ReadAsStringAsync();
        var groupContent = await groupResponse.Content.ReadAsStringAsync();
        List<UserDto> result = [];
        var groupResult = JsonConvert.DeserializeObject<GraphUsersResponse>(groupContent)!.Users;
        var userResult= JsonConvert.DeserializeObject<GraphUsersResponse>(userContent)!.Users;
        result.AddRange(groupResult);
        result.AddRange(userResult);        
        result = result.Where(i => i.Email != null).ToList();
        
        if (eventId == null) { return result; }       
        
        var participantsForEvent = _uow.EventRepository.GetParticipantsByEventId(eventId.Value);
        var participantIdsForEvent = participantsForEvent.Select(p => p.UserId).ToHashSet();
        return [.. result.Where(u => !participantIdsForEvent.Contains(u.UserId))];
    }
     

    public async Task<UserDto?> GetUserWithId(Guid userId)
    {
        await SetAccessToken();
        var searchId = userId.ToString();
        var response = await _httpClient.GetAsync($"users/{searchId}");
        if (response.IsSuccessStatusCode == false)
        {
            return null;
        }

        var content = await response.Content.ReadAsStringAsync();
        var userDto = JsonConvert.DeserializeObject<UserDto>(content);
        if (userDto is null || userDto.Username.Length < 1)
        {
            return null;
        }
        return userDto;
    }

    public async Task<List<string>> GetNamesWithIds(List<Guid> userIds)
    {
        throw new NotImplementedException();
    }

    public async Task SendEmail(List<Guid> userIds, EmailTemplate message)
    {
        Collection<string> recipients = [];
        foreach (var userId in userIds)
        {
            var recipient = await GetUserWithId(userId);
            var recipientEmail = recipient.Email;
            recipients.Add(recipientEmail);
        }
        await SetAccessToken();
        var mailPayload = new
        {
            message = new
            {
                subject = message.Subject,
                body = new
                {
                    contentType = "HTML",
                    content = message.Body
                },
                toRecipients = recipients.Select(email => new { emailAddress = new { address = email } }).ToArray()
            },
            saveToSentItems = false
        };
        var jsonPayload = JsonConvert.SerializeObject(mailPayload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var requestUri = $"users/{_config["Graph:SenderEmail"]}/sendMail";
        var response = await _httpClient.PostAsync(requestUri, content);
        response.EnsureSuccessStatusCode();
    }

    public async Task <List<UserDto>> GetUsersFromIds(Guid[] userIds)
    {
        Collection<UserDto> users = [];
        foreach (Guid id in userIds)
        {
            var user = await GetUserWithId(id);
            if (user is not null)
            {
                users.Add(user);
            }
        }
        return [.. users];
    }

    public async Task<List<UserDto>> GetUsersFromGroup(Guid groupId)
    {
        var group = await _httpClient.GetAsync($"groups/{groupId}?$expand=members($select=id,displayName,mail)");
        if (group.IsSuccessStatusCode)
        {
            var groupContent = await group.Content.ReadAsStringAsync();
            var groupResult = JsonConvert.DeserializeObject<GraphGroupResponse>(groupContent)!.Members.Users;
            return [.. groupResult];
        }
        return [];
    }

    private async Task SetAccessToken()
    {
        if (string.IsNullOrEmpty(_accessToken))
        {
            _accessToken = await _graphTokenService.GetAccessToken();
        }
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
    }
    
    public class GraphUsersResponse
    {
        [JsonProperty("value")]
        public UserDto[] Users { get; set; }
    }

    public class GraphGroupResponse
    {
        [JsonProperty("members")]
        public GraphUsersResponse Members { get; set; }
    }
}