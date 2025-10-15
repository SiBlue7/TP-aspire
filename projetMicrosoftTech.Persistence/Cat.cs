using System.ComponentModel.DataAnnotations.Schema;

namespace projetMicrosoftTech.Persistence;

[Table("cat")]
public class Cat
{
    public int Id { get; set; }
    public string name { get; set; }
}
