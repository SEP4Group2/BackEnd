using Domain.DTOs;
using Domain.Model;

namespace DataAccess.DAOInterfaces;

public interface IPlantDataDAO
{
    Task<PlantData> SaveAsync(PlantDataCreationDTO plantData);
    Task<List<PlantData>> GetAllByPlantIdAsync(int id);
    Task<List<PlantData>> GetAllByUserId(int userId);

}