using projetMicrosoftTech.Persistence;

namespace projetMicrosoftTech.WebApp.Clients;

public interface ICatClient
{
    Task<List<Cat>> GetCatItemsAsync();
    Task<Cat> CreateCatItemAsync(Cat item);
}