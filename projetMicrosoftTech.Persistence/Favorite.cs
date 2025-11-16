using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace projetMicrosoftTech.Persistence;

[Table("favorite")]
public class Favorite
{
    public int id { get; set; }

    public string userId { get; set; } = default!;

    public int catId { get; set; }
    
    [JsonIgnore]
    [ForeignKey("catId")]
    public Cat cat { get; set; }
}