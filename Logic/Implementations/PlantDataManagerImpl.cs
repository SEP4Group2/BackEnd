using System.Net.Http.Json;
using DataAccess.DAOInterfaces;
using Domain.DTOs;
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
    public async Task<PlantData> SaveAsync(PlantDataCreationDTO plantData)
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

    public async Task CheckDataWithPlantPreset(PlantData plantData)
    {
        Console.WriteLine(plantData.PlantDevice);
        Console.WriteLine(plantData);
        PlantPreset optimalPreset = plantData.PlantDevice.Plant.PlantPreset;
        string currentUserId = plantData.PlantDevice.Plant.User.UserId.ToString();
        
        // I am making this up, we can change the numbers later so it's more accurate
        if (Math.Abs(optimalPreset.Humidity - plantData.Humidity) > 50)
        {
            await SendNotification(new NotificationRequestDTO()
            {
                UserId = currentUserId,
                Message = "Humidity levels are currently out of optimal range. Please check your plant"
            });
        }
        
        if (Math.Abs(optimalPreset.Temperature - plantData.Temperature) > 50)
        {
            await SendNotification(new NotificationRequestDTO()
            {
                UserId = currentUserId,
                Message = "Temperature levels are currently out of optimal range. Please check your plant"
            });
        }
        
        if (Math.Abs(optimalPreset.UVLight- plantData.UVLight) > 50)
        {
            await SendNotification(new NotificationRequestDTO()
            {
                UserId = currentUserId,
                Message = "UV Light levels are currently out of optimal range. Please check your plant"
            });
        }
        
        if (Math.Abs(optimalPreset.Moisture- plantData.Moisture) > 50)
        {
            await SendNotification(new NotificationRequestDTO()
            {
                UserId = currentUserId,
                Message = "Moisture levels are currently out of optimal range. Please check your plant"
            });
        }
    }

    public async Task SendNotification(NotificationRequestDTO dto)
    {
        try
        {
            using (var client = new HttpClient())
            {
                await client.PostAsJsonAsync("http://notificationserver:5016/notification/send", dto);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error occurred: {e}");
        }
    }

}