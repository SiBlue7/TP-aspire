using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projetMicrosoftTech.Persistence;

namespace projetMicrosoftTech.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CatsController : ControllerBase
{
    private readonly MyAppDbContext _db;
    
    private string GetUserId() =>
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? throw new Exception("Impossible de récupérer l'id utilisateur");

    public CatsController(MyAppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetCats()
    {
        var cats = await _db.Cat
            .Include(c => c.photos)
            .ToListAsync();

        return Ok(cats);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCat([FromBody] Cat item)
    {
        var userId = GetUserId();
        
        item.createdByUserId = userId;
        
        _db.Cat.Add(item);
        await _db.SaveChangesAsync();

        return Created($"/api/cats/{item.id}", item);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCat(int id)
    {
        var cat = await _db.Cat.FindAsync(id);
        if (cat == null)
            return NotFound();

        if (cat.createdByUserId != GetUserId())
            return Forbid();

        _db.Cat.Remove(cat);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}