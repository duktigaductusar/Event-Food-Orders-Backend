using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text;
using EventFoodOrders.Dto.GraphDTOs;
using EventFoodOrders.Dto.UserDTOs;
using EventFoodOrders.Repositories.Interfaces;
using EventFoodOrders.Services.Interfaces;
using EventFoodOrders.Utilities;
using Newtonsoft.Json;

namespace EventFoodOrders.Services;

/**
 * TODO! Batch requests instead of submitting multiple requests 
 */
public class UserService : IUserService
{
    // Batch info: https://learn.microsoft.com/en-us/graph/json-batching?tabs=http#json-batching-restrictions
    private readonly int _graphBatchLimit = 20;
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
        var groupResult = JsonConvert.DeserializeObject<GraphUsersResponseDto>(groupContent)!.Users;
        var userResult= JsonConvert.DeserializeObject<GraphUsersResponseDto>(userContent)!.Users;
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
        List<UserDto> allUsers = [];
        foreach (var chunk in userIds.Chunk(_graphBatchLimit))
        {
            var users = await GetUsersFromIdsBatch(chunk);
            allUsers.AddRange(users);
        }
        return allUsers;
    }

    private async Task<List<UserDto>> GetUsersFromIdsBatch(Guid[] userIds)
    {
        await SetAccessToken();

        var batchRequests = userIds.Select((id, index) => new
        {
            id = (index + 1).ToString(),
            method = "GET",
            url = $"/users/{id}"
        }).ToList();

        var batchPayload = new
        {
            requests = batchRequests
        };

        var requestContent = new StringContent(
            JsonConvert.SerializeObject(batchPayload),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync("$batch", requestContent);

        if (response.IsSuccessStatusCode == false)
        {
            return [];
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var batchResponse = JsonConvert.DeserializeObject<GraphBatchResponseDto>(responseContent);

        return batchResponse?.Responses
            .Where(r => r.Status == 200)
            .Select(r => JsonConvert.DeserializeObject<UserDto>(r.Body.ToString()))
            .Where(u => u is not null)
            .ToList()!;
    }

    public async Task<List<Guid>> GetUsersFromGroup(Guid groupId)
    {
        var groupResponse = await _httpClient.GetAsync($"groups/{groupId}/members");
        if (groupResponse.IsSuccessStatusCode)
        {
            var groupContent = await groupResponse.Content.ReadAsStringAsync();
            var groupAsJson = JsonConvert.DeserializeObject<GraphGroupResponseDto>(groupContent)!;
            var members = groupAsJson.Members.Where(m => m.Mail is not null);
            var users = members.Select(m => m.Id).ToList();
            return [.. users];
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
}