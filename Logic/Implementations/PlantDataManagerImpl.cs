using DataAccess.DAOInterfaces;
using Domain.Model;
using Logic.Interfaces;

namespace Logic.Implementations;

public class PlantDataManagerImpl : IPlantDataManager
{

    private IPlantDataDAO plantDataDao;

    public PlantDataManagerImpl(IPlantDataDAO plantDataDao)
    {
        this.plantDataDao = plantDataDao;
    }
    public async Task<PlantData> SaveAsync(PlantData plantData)
    {
        return await plantDataDao.SaveAsync(plantData);
    }
}