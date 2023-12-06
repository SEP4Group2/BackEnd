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

    public async Task<List<PlantData>> FetchPlantDataAsync(int userId)
    {
        try
        {
            var groupedData = await _appContext.PlantData
                .Where(p => p.PlantDevice.Plant!.User.UserId == userId)
                .Include(p => p.PlantDevice.Plant)
                .GroupBy(p => p.PlantDevice.Plant!.PlantId)
                .ToListAsync();

            var fetchedPlantData = groupedData
                .SelectMany(g => g.OrderByDescending(p => DateTime.Parse(p.TimeStamp)).Take(1))
                .ToList();

            return fetchedPlantData;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new Exception("Error fetching data from plantdb");
        }
    }

    public async Task<List<PlantData>> GetPlantDataByPlantIdAsync(int plantId)
    {
        try
        {
            //get the device that is associated to the plant
            Device connectedDevice = _appContext.Devices.FirstOrDefault(d => d.Plant.PlantId == plantId);
            //return just plantData that are associated to this particular device, thus plant
            return await _appContext.PlantData.Where(pd => pd.PlantDevice.DeviceId == connectedDevice.DeviceId)
                .Include(pd => pd.PlantDevice.Plant)
                .ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}