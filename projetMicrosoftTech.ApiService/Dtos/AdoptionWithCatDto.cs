using projetMicrosoftTech.ApiService.Controllers;
using projetMicrosoftTech.Persistence;

namespace projetMicrosoftTech.ApiService.Dtos;

public class AdoptionWithCatDto
{
    public int id { get; set; }
    public string comment { get; set; }
    public AdoptionStatus status { get; set; }
    public string askedByUserId { get; set; }
    public int catId { get; set; }
    public CatDto cat { get; set; }
}