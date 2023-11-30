using DataAccess.DAOInterfaces;
using Domain.DTOs;
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
    
    public async Task<PlantData> SaveAsync(PlantDataCreationDTO plantData)
    {
        Device? existingDevice = await Task.FromResult(_appContext.Devices.Include(d => d.Plant)
            .ThenInclude(p => p.PlantPreset).Include(d=> d.Plant).ThenInclude(p=> p.User).FirstOrDefault(d => d.DeviceId == plantData.DeviceId));
        if (existingDevice == null) throw new Exception("Device not found");
        
        var newPlantData = new PlantData()
        {
            Humidity = plantData.Humidity,
            Temperature = plantData.Temperature, 
            Moisture = plantData.Moisture, 
            UVLight = plantData.UVLight,
            PlantDevice = existingDevice,
            TimeStamp = plantData.TimeStamp,
            TankLevel = plantData.TankLevel
        };
        
        EntityEntry<PlantData> plantDataEntity = await _appContext.PlantData.AddAsync(newPlantData);
        await _appContext.SaveChangesAsync();
        return plantDataEntity.Entity;
    }

    public async Task<List<PlantData>> GetAllByPlantIdAsync(int id)
    {
        try
        {
            List<PlantData> fetchedPlantData = await _appContext.PlantData.Include(pd => pd.PlantDevice).ThenInclude(p=>p.Plant)
                .ThenInclude(p=>p.PlantPreset).ToListAsync();
            return fetchedPlantData;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error fetching data from plantdb");
            throw new Exception("Error fetching data from plantdb");
        }
    }
}