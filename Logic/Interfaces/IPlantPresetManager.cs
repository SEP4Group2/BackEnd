using Domain.DTOs;
using Domain.Model;

namespace Logic.Interfaces;

public interface IPlantPresetManager
{
    Task<PlantPreset> CreateAsync(PlantPresetCreationDTO plantCreationDto);
    Task<PlantPreset> GetByIdAsync(int presetId);
    
    Task<IEnumerable<PlantPreset>> GetAllPlantPresetsAsync();

    Task<IEnumerable<PlantPreset>> GetPresetsByUserIdAsync(int userId);

}