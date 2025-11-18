using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace projetMicrosoftTech.Persistence;

public enum AdoptionStatus
{
    EnAttente,
    Valide,
    Refuse
}

[Table("adoption")]
public class Adoption
{
    public int id { get; set; }
    public string? comment { get; set; }
    public AdoptionStatus status { get; set; }
    public string askedByUserId { get; set; }
    public string askedByUserName { get; set; }
    public int catId { get; set; }
    
    [JsonIgnore]
    [ForeignKey("catId")]
    public Cat cat { get; set; }
}