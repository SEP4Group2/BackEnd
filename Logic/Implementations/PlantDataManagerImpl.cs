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
        try
        {
            return await plantDataDao.SaveAsync(plantData);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<PlantData>> GetAllByPlantIdAsync(int id)
    {
        return await plantDataDao.GetAllByPlantIdAsync(id);
    }
    
}