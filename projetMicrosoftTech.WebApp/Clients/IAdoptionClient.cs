using projetMicrosoftTech.Persistence;
using projetMicrosoftTech.WebApp.Dtos;

namespace projetMicrosoftTech.WebApp.Clients;

public interface IAdoptionClient
{
    Task<bool> RequestAdoptionAsync(int catId, string? comment);
    Task<List<Adoption>> GetMyAdoptionRequestsAsync();
    Task<List<AdoptionWithCatDto>> GetAdoptionRequestsForMyCatsAsync();
    Task<bool> UpdateStatusAsync(int id, AdoptionStatus status);
}