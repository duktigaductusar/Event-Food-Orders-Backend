using System.Net.Http.Headers;
using Azure.Core;
using EventFoodOrders.Dto.UserDTOs;
using EventFoodOrders.Repositories.Interfaces;
using EventFoodOrders.Services;
using Newtonsoft.Json;

namespace EventFoodOrders.Repositories;

public class GraphRepository : IGraphRepository
{
    private readonly IGraphTokenService _graphTokenService;
    private readonly HttpClient _httpClient;

    public GraphRepository(IGraphTokenService graphTokenService, HttpClient httpClient)
    {
        _graphTokenService = graphTokenService;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://graph.microsoft.com/v1.0/");
    }

    public async Task<UserDto> GetUserAsync(Guid userId)
    {
        var searchId = userId.ToString();
        var accessToken = await _graphTokenService.GetAccessToken();
        Console.WriteLine("######## Graph Access Token ########");
        Console.WriteLine(accessToken);
        Console.WriteLine("####################################");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        
        var debug = new HttpRequestMessage(HttpMethod.Get, $"users/{searchId}");
        debug.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        Console.WriteLine("######## Debug Request Headers ########");
        Console.WriteLine(debug.Headers.ToString());
        Console.WriteLine("#######################################");
        
        var response = await _httpClient.GetAsync($"users/{searchId}");
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<UserDto>(content);
    }
}