using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model;

public class Plant
{
    [Key]
    public int PlantId { get; set; }
    
    [MaxLength(50)]
    public string Name { get; set; }

    [MaxLength(50)]
    public string Location { get; set; }
    
    public string Type { get; set; }
    
    public PlantPreset PlantPreset { get; set; }
    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; }
}