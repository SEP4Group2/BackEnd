using Domain.DTOs;
using Domain.Model;

namespace DataAccess.DAOInterfaces;

public interface IPlantDataDAO
{
    Task<PlantData> SaveAsync(PlantDataCreationListDTO plantDataList);
    Task<List<PlantData>> FetchPlantDataAsync(int userId);

    Task<List<PlantData>> GetPlantDataByPlantIdAsync(int plantId);

}