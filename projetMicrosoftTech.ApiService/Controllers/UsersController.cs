using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace projetMicrosoftTech.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private string GetUserId() =>
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new Exception("Impossible de récupérer l'ID utilisateur");

        [HttpGet("me")]
        public IActionResult GetCurrentUserId()
        {
            var userId = GetUserId();
            return Ok(new { userId });
        }
    }
}