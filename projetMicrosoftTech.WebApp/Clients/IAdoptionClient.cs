using projetMicrosoftTech.Persistence;
using projetMicrosoftTech.WebApp.Dtos;

namespace projetMicrosoftTech.WebApp.Clients;

public interface IAdoptionClient
{
    Task<bool> RequestAdoptionAsync(int catId, string? comment);
    Task<List<AdoptionWithCatDto>> GetMyAdoptionRequestsAsync();
    Task<List<AdoptionWithCatDto>> GetAdoptionRequestsForMyCatsAsync();
    Task<bool> UpdateStatusAsync(int id, AdoptionStatus status);
}