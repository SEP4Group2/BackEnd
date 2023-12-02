namespace Domain.DTOs;

public class DeviceRegistrationDTO
{
    public int DeviceId { get; set; }
    
    public bool Status { get; set; }

    public int? PlantId { get; set; }
}