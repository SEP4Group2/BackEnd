using DataAccess.DAOInterfaces;
using Domain.Model;
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
}