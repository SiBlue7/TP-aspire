using System.Net.Http.Json;
using projetMicrosoftTech.WebApp.Clients;

public class UserClient : IUserClient
{
    private readonly HttpClient _httpClient;

    public UserClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetCurrentUserIdAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<UserDto>("/api/user/me");
        return response?.UserId ?? string.Empty;
    }

    private class UserDto
    {
        public string UserId { get; set; } = string.Empty;
    }
}