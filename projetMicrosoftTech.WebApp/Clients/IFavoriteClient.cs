using projetMicrosoftTech.Persistence;

namespace projetMicrosoftTech.WebApp.Clients;

public interface IFavoritesClient
{
    Task<List<Favorite>> GetMyFavoritesAsync();
    Task AddFavoriteAsync(int catId);
    Task RemoveFavoriteAsync(int catId);
}