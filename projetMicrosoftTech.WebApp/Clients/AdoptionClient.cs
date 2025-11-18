using System.Text.Json;
using projetMicrosoftTech.Persistence;
using projetMicrosoftTech.WebApp.Dtos;

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

    public async Task<List<AdoptionWithCatDto>> GetMyAdoptionRequestsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<AdoptionWithCatDto>>("/api/adoption/my-requests") ?? new();
    }

    public async Task<List<AdoptionWithCatDto>> GetAdoptionRequestsForMyCatsAsync()
    {
        var list = await _httpClient.GetFromJsonAsync<List<AdoptionWithCatDto>>("/api/adoption/for-my-cats") 
                   ?? new List<AdoptionWithCatDto>();

        Console.WriteLine("=== Adoption Requests ===");
        Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(list, new JsonSerializerOptions
        {
            WriteIndented = true
        }));

        return list;
    }

    
    public async Task<bool> UpdateStatusAsync(int id, AdoptionStatus status)
    {
        var obj = new { Status = status };
        var response = await _httpClient.PutAsJsonAsync($"/api/adoption/{id}/status", obj);
        return response.IsSuccessStatusCode;
    }
}