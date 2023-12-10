using Domain.Model;

namespace Domain.DTOs;

public class GetAllPlantsDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public int DeviceId { get; set; }
    public PlantPreset PlantPreset { get; set; }
    public int IconId { get; set; }

    public GetAllPlantsDTO(int id, string name, string location, PlantPreset plantPreset, int deviceId, int iconId)
    {
        Id = id;
        Name = name;
        Location = location;
        PlantPreset = plantPreset;
        DeviceId = deviceId;
        IconId = iconId;
    }
}