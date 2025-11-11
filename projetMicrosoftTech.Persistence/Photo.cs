using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace projetMicrosoftTech.Persistence;

[Table("photo")]
public class Photo
{
    public int id { get; set; }

    public int catId { get; set; }
    public string photoUrl { get; set; }

    [JsonIgnore]
    [ForeignKey("catId")]
    public Cat cat { get; set; }
}