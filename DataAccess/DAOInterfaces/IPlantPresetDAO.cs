using Domain.DTOs;
using Domain.Model;

namespace DataAccess.DAOInterfaces;

public interface IPlantPresetDAO
{
    Task<PlantPreset> CreateAsync(PlantPresetCreationDTO plant);

    Task<PlantPreset> GetAsync(int id);

    Task<List<PlantPreset>> GetAllPlantPresentsAsync();

    Task<List<PlantPreset>> GetPresetsByUserIdAsync(int userId);

}
