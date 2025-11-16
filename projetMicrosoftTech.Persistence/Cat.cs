using System.ComponentModel.DataAnnotations.Schema;

namespace projetMicrosoftTech.Persistence;

[Table("cat")]
public class Cat
{
    public int id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public int age { get; set; }
    public string sex { get; set; }
    public string createdByUserId { get; set; }

    public List<Photo> photos { get; set; } = new();
    public List<Favorite> favorites { get; set; } = new();
    public List<Adoption> Adoptions { get; set; } = new();
}
