using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projetMicrosoftTech.Persistence;
using System.Security.Claims;

namespace projetMicrosoftTech.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FavoritesController : ControllerBase
{
    private readonly MyAppDbContext _db;

    public FavoritesController(MyAppDbContext db)
    {
        _db = db;
    }

    private string GetUserId() =>
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? throw new Exception("Impossible de récupérer l'id utilisateur");

    [HttpGet]
    public async Task<IActionResult> GetMyFavorites()
    {
        var userId = GetUserId();

        var favs = await _db.Favorite
            .Where(f => f.userId == userId)
            .Include(f => f.cat)
            .ToListAsync();

        return Ok(favs);
    }

    [HttpPost("{catId}")]
    public async Task<IActionResult> AddFavorite(int catId)
    {
        var userId = GetUserId();

        if (!await _db.Cat.AnyAsync(c => c.id == catId))
            return NotFound("Chat introuvable");

        var already = await _db.Favorite
            .AnyAsync(f => f.catId == catId && f.userId == userId);

        if (already)
            return Conflict("Ce chat est déjà dans vos favoris.");

        var fav = new Favorite
        {
            userId = userId,
            catId = catId
        };

        _db.Favorite.Add(fav);
        await _db.SaveChangesAsync();

        return Created($"/api/favorites/{fav.id}", fav);
    }

    [HttpDelete("{catId}")]
    public async Task<IActionResult> RemoveFavorite(int catId)
    {
        var userId = GetUserId();

        var fav = await _db.Favorite
            .FirstOrDefaultAsync(f => f.catId == catId && f.userId == userId);

        if (fav == null)
            return NotFound("Ce chat n'est pas dans vos favoris.");

        _db.Favorite.Remove(fav);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
