using System.ComponentModel.DataAnnotations;

namespace Domain.Model;

public class Plant
{
    [Key]
    public int PlantId { get; set; }
    
    [MaxLength(50)]
    public string Name { get; set; }

    [MaxLength(50)]
    public string Location { get; set; }

    public PlantPreset PlantPreset { get; set; }
    
    public User User { get; set; }
    
   
}