using System.Net.Http.Json;
using projetMicrosoftTech.Persistence;

namespace projetMicrosoftTech.WebApp.Clients;

public class FavoritesClient : IFavoritesClient
{
    private readonly HttpClient _httpClient;

    public FavoritesClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Favorite>> GetMyFavoritesAsync()
    {
        var favorites = await _httpClient.GetFromJsonAsync<List<Favorite>>("api/favorites");
        return favorites ?? new List<Favorite>();
    }

    public async Task AddFavoriteAsync(int catId)
    {
        var response = await _httpClient.PostAsync($"api/favorites/{catId}", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveFavoriteAsync(int catId)
    {
        var response = await _httpClient.DeleteAsync($"api/favorites/{catId}");
        response.EnsureSuccessStatusCode();
    }
}