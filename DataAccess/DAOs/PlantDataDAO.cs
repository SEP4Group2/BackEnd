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
    
    public async Task SaveAsync(PlantDataCreationListDTO plantDataList)
    {
        Device? existingDevice = await Task.FromResult(_appContext.Devices.Include(d => d.Plant)
            .ThenInclude(p => p.PlantPreset).Include(d=> d.Plant).ThenInclude(p=> p.User).FirstOrDefault(d => d.DeviceId == plantDataList.PlantDataApi.First().DeviceId));
        if (existingDevice == null) throw new Exception("Device not found");

        List<PlantData> plantData = new();

        foreach (PlantDataCreationDTO plantDataCreationDto in plantDataList.PlantDataApi)
        {
            var newPlantData = new PlantData()
            {
                Humidity = plantDataCreationDto.Humidity,
                Temperature = plantDataCreationDto.Temperature, 
                Moisture = plantDataCreationDto.Moisture, 
                UVLight = plantDataCreationDto.UVLight,
                PlantDevice = existingDevice,
                TimeStamp = plantDataCreationDto.TimeStamp,
                TankLevel = plantDataCreationDto.TankLevel
            };
            plantData.Add(newPlantData);
        }
        
        
        await _appContext.PlantData.AddRangeAsync(plantData);
        await _appContext.SaveChangesAsync();
    }

    public async Task<List<PlantData>> FetchPlantDataAsync(int userId)
    {
        try
        {
            var groupedData = await _appContext.PlantData
            .Where(p => p.PlantDevice.Plant!.User.UserId == userId)
            .Include(p => p.PlantDevice.Plant).ThenInclude(p=>p.PlantPreset)
            .ToListAsync();

        var fetchedPlantData = groupedData
            .GroupBy(p => p.PlantDevice.Plant!.PlantId)
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