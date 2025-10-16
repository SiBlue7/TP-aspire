using projetMicrosoftTech.Persistence;

namespace projetMicrosoftTech.WebApp.Clients;

public class CatClient : ICatClient
{
    private readonly HttpClient _httpClient;
    public CatClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<List<Cat>> GetCatItemsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Cat>>("/api/cats");
    }
    public async Task<Cat> CreateCatItemAsync(Cat item)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/cats", item);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Cat>();
    }
}