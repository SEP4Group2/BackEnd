using System.ComponentModel.DataAnnotations;

namespace Domain.Model;

public class PlantPreset
{
    
    [Key]
    public int PresetId { get; set; }

    public string Name { get; set; }

    public float Humidity { get; set; }

    public float Temperature { get; set; }
    
    public float UVLight { get; set; }
    
    public float Moisture { get; set; }

    

}