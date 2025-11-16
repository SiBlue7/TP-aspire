using projetMicrosoftTech.Persistence;

namespace projetMicrosoftTech.WebApp.Clients;

public interface IAdoptionClient
{
    Task<bool> RequestAdoptionAsync(int catId, string? comment);
    Task<List<Adoption>> GetMyAdoptionRequestsAsync();
    Task<List<Adoption>> GetAdoptionRequestsForMyCatsAsync();
}