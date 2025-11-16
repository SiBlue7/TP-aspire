using projetMicrosoftTech.Persistence;

namespace projetMicrosoftTech.WebApp.Clients;

public interface ICatClient
{
    Task<List<Cat>> GetCatItemsAsync();
    Task<Cat> CreateCatItemAsync(Cat item);
    
    Task UploadCatPhotoAsync(int catId, string fileName, byte[] fileData);
    Task<bool> DeleteCatItemAsync(int catId);

}