using Domain.Model;

namespace DataAccess.DAOInterfaces;

public interface IPlantDataDAO
{
    Task<PlantData> SaveAsync(PlantData plantData);
}