namespace Domain.DTOs;

public class PlantDataCreationDTO
{
    public float Humidity { get; set; }

    public float Temperature { get; set; }
    
    public float UVLight { get; set; }
    
    public float Moisture { get; set; }
    
    public string TimeStamp { get; set; }

    public float TankLevel { get; set; }
    
    public int DeviceId { get; set; }
}