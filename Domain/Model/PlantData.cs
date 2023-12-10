using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Model;

public class PlantData
{
    [Key]
    public int PlantDataId { get; set; }
    public float Humidity { get; set; }

    public float Temperature { get; set; }
    
    public float UVLight { get; set; }
    
    public float Moisture { get; set; }
    
    public float TankLevel { get; set; }
    
    public string TimeStamp { get; set; }
    
    public Device PlantDevice { get; set; }

    public int? PercentageStatus { get; set; }
    
    
}
