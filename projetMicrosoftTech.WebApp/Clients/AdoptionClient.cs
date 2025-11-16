using projetMicrosoftTech.Persistence;

namespace projetMicrosoftTech.WebApp.Clients;

public class AdoptionClient : IAdoptionClient
{
    private readonly HttpClient _httpClient;

    public AdoptionClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> RequestAdoptionAsync(int catId, string? comment)
    {
        var request = new { CatId = catId, Comment = comment };
        var response = await _httpClient.PostAsJsonAsync("/api/adoption", request);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<Adoption>> GetMyAdoptionRequestsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Adoption>>("/api/adoption/my-requests") ?? new();
    }

    public async Task<List<Adoption>> GetAdoptionRequestsForMyCatsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Adoption>>("/api/adoption/for-my-cats") ?? new();
    }
}