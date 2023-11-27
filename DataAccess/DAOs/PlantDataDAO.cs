using DataAccess.DAOInterfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataAccess.DAOs;

public class PlantDataDAO : IPlantDataDAO
{
    private readonly AppContext _appContext;

    public PlantDataDAO(AppContext _appContext)
    {
        this._appContext = _appContext;
    }
    
    public async Task<PlantData> SaveAsync(PlantData plantData)
    {
        EntityEntry<PlantData> newPlantData =  await _appContext.PlantData.AddAsync(plantData);
        await _appContext.SaveChangesAsync();
        return newPlantData.Entity;
    }

    public async Task<List<PlantData>> GetAllByPlantIdAsync(int id)
    {
        try
        {
            List<PlantData> fetchedPlantData = await _appContext.PlantData.ToListAsync();
            return fetchedPlantData;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error fetching data from plantdb");
            throw new Exception("Error fetching data from plantdb");
        }
    }
}