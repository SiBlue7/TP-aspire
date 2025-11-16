using System.Net.Http.Headers;
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
    public async Task UploadCatPhotoAsync(int catId, string fileName, byte[] fileData)
    {
        using var content = new MultipartFormDataContent();
        var byteArrayContent = new ByteArrayContent(fileData);
    
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        var mimeType = extension switch
        {
            ".png" => "image/png",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".gif" => "image/gif",
            _ => "application/octet-stream"
        };
    
        byteArrayContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);
        content.Add(byteArrayContent, "file", fileName);
    
        var response = await _httpClient.PostAsync($"/api/cats/{catId}/photos", content);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task<bool> DeleteCatItemAsync(int catId)
    {
        var response = await _httpClient.DeleteAsync($"/api/cats/{catId}");
        return response.IsSuccessStatusCode;
    }
}