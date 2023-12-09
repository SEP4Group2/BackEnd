using Domain.DTOs;
using Domain.Model;

namespace DataAccess.DAOInterfaces;

public interface IPlantDAO
{
    Task<Plant> CreateAsync(PlantCreationDTO plant);
    Task<Plant> GetAsync(int id);
    Task<List<GetAllPlantsDTO>> GetAllPlantsAsync(int userId);
    Task RemoveAsync(int id);
    Task<Plant> EditAsync(EditPlantDTO editPlantDto);

}