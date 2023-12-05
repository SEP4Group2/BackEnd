using Domain.Model;

namespace Domain.DTOs;

public class PlantCreationDTO
{
    public string Name { get; set; }
    public string Location { get; set; }
    public int PlantPresetId { get; set; }
    public int UserId { get; set; }
    public int DeviceId { get; set; }
    public int IcondId { get; set; }
}
