using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projetMicrosoftTech.Persistence;

namespace projetMicrosoftTech.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdoptionController : ControllerBase
{
    private readonly MyAppDbContext _db;

    public AdoptionController(MyAppDbContext db)
    {
        _db = db;
    }

    private string GetUserId() =>
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? throw new Exception("Impossible de récupérer l'id utilisateur");

    // POST /api/adoption
    [HttpPost]
    public async Task<IActionResult> RequestAdoption([FromBody] CreateAdoptionRequest request)
    {
        var userId = GetUserId();

        var cat = await _db.Cat.FindAsync(request.CatId);
        if (cat == null)
            return NotFound("Chat introuvable.");

        if (cat.createdByUserId == userId)
            return BadRequest("Vous ne pouvez pas adopter votre propre chat.");

        var alreadyRequested = await _db.Adoption
            .AnyAsync(a => a.catId == request.CatId 
                        && a.askedByUserId == userId 
                        && a.status == AdoptionStatus.EnAttente);

        if (alreadyRequested)
            return Conflict("Vous avez déjà une demande en attente pour ce chat.");

        var adoption = new Adoption
        {
            catId = request.CatId,
            askedByUserId = userId,
            comment = request.Comment,
            status = AdoptionStatus.EnAttente
        };

        _db.Adoption.Add(adoption);
        await _db.SaveChangesAsync();

        return Created($"/api/adoption/{adoption.id}", adoption);
    }

    // GET /api/adoption/my-requests
    [HttpGet("my-requests")]
    public async Task<IActionResult> GetMyRequests()
    {
        var userId = GetUserId();

        var adoptions = await _db.Adoption
            .Where(a => a.askedByUserId == userId)
            .Include(a => a.cat)
                .ThenInclude(c => c.photos)
            .OrderByDescending(a => a.id)
            .ToListAsync();

        return Ok(adoptions);
    }

    // GET /api/adoption/for-my-cats
    [HttpGet("for-my-cats")]
    public async Task<IActionResult> GetRequestsForMyCats()
    {
        var userId = GetUserId();

        var adoptions = await _db.Adoption
            .Include(a => a.cat)
                .ThenInclude(c => c.photos)
            .Where(a => a.cat.createdByUserId == userId)
            .OrderByDescending(a => a.id)
            .ToListAsync();

        return Ok(adoptions);
    }

    // PUT /api/adoption/{id}/status
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request)
    {
        var adoption = await _db.Adoption
            .Include(a => a.cat)
            .FirstOrDefaultAsync(a => a.id == id);

        if (adoption == null)
            return NotFound("Demande d'adoption introuvable.");

        if (adoption.cat.createdByUserId != GetUserId())
            return Forbid();

        adoption.status = request.Status;
        await _db.SaveChangesAsync();

        return Ok(adoption);
    }
}

public record CreateAdoptionRequest(int CatId, string? Comment);
public record UpdateStatusRequest(AdoptionStatus Status);