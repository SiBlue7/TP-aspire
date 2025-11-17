using projetMicrosoftTech.ApiService.Controllers;

namespace projetMicrosoftTech.ApiService.Dtos;

public class CatDto
{
    public int id { get; set; }
    public string name { get; set; }
    public List<PhotoDto> photos { get; set; }
}