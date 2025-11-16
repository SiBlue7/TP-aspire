using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projetMicrosoftTech.Persistence;

namespace projetMicrosoftTech.ApiService.Controllers;

[ApiController]
[Route("api/cats/{catId}/[controller]")]
[Authorize]
public class PhotosController : ControllerBase
{
    private readonly MyAppDbContext _db;
    private readonly IWebHostEnvironment _env;

    public PhotosController(MyAppDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    // POST /api/cats/{catId}/photos
    [HttpPost]
    public async Task<IActionResult> UploadPhoto(int catId, [FromForm] IFormFile file)
    {
        // Vérifier que le chat existe
        if (!await _db.Cat.AnyAsync(c => c.id == catId))
            return NotFound($"Chat avec l'id {catId} introuvable.");

        if (file == null || file.Length == 0)
            return BadRequest("Aucun fichier envoyé.");

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
            return BadRequest("Type de fichier non autorisé.");

        var uploadsFolder = Path.Combine(_env.ContentRootPath, "wwwroot", "images");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var filePath = Path.Combine(uploadsFolder, file.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var photo = new Photo
        {
            catId = catId,
            photoUrl = file.FileName,
        };

        _db.Photo.Add(photo);
        await _db.SaveChangesAsync();

        return Created($"/api/cats/{catId}/photos/{photo.id}", photo);
    }
}