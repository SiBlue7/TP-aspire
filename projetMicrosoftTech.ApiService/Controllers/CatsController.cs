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
        _db.Cat.Add(item);
        await _db.SaveChangesAsync();

        return Created($"/api/cats/{item.id}", item);
    }
}