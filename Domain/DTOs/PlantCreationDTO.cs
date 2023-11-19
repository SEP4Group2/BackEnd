using Domain.Model;

namespace Domain.DTOs;

public class PlantCreationDTO
{
    public string Name { get; set; }
    public string Location { get; set; }
    public int PlantPresetId { get; set; }
}
