using System.Net.Http.Json;
using DataAccess.DAOInterfaces;
using Domain.DTOs;
using Domain.Model;
using Logic.Interfaces;

namespace Logic.Implementations;

public class PlantDataManagerImpl : IPlantDataManager
{

    private IPlantDataDAO plantDataDao;
    private INotificationSender notificationSener;

    public PlantDataManagerImpl(IPlantDataDAO plantDataDao, INotificationSender notificationSener)
    {
        this.plantDataDao = plantDataDao;
        this.notificationSener = notificationSener;
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
    
    public async Task CheckDataWithPlantPreset(PlantData plantData)
    {
        int maxDifferenceAllowed = 50;
        PlantPreset optimalPreset = plantData.PlantDevice.Plant.PlantPreset;
        string name = plantData.PlantDevice.Plant.Name;
        string currentUserId = plantData.PlantDevice.Plant.User.UserId.ToString();
        
        // I am making this up, we can change the numbers later so it's more accurate
        if (Math.Abs(optimalPreset.Humidity - plantData.Humidity) > maxDifferenceAllowed)
        {
            await notificationSener.SendNotification(new NotificationRequestDTO()
            {
                UserId = currentUserId,
                Message = $"Humidity levels of plant {name} are currently out of optimal range. Please check your plant"
            });
        }
        
        if (Math.Abs(optimalPreset.Temperature - plantData.Temperature) > maxDifferenceAllowed)
        {
            await notificationSener.SendNotification(new NotificationRequestDTO()
            {
                UserId = currentUserId,
                Message = $"Temperature levels of plant {name} are currently out of optimal range. Please check your plant"
            });
        }
        
        if (Math.Abs(optimalPreset.UVLight- plantData.UVLight) > maxDifferenceAllowed)
        {
            await notificationSener.SendNotification(new NotificationRequestDTO()
            {
                UserId = currentUserId,
                Message = $"UV Light levels of plant {name} are currently out of optimal range. Please check your plant"
            });
        }
        
        if (Math.Abs(optimalPreset.Moisture- plantData.Moisture) > maxDifferenceAllowed)
        {
            await notificationSener.SendNotification(new NotificationRequestDTO()
            {
                UserId = currentUserId,
                Message = $"Moisture levels of plant {name} are currently out of optimal range. Please check your plant"
            });
        }
    }
    public async Task<List<PlantData>> FetchPlantDataAsync(int userId)
    {
        return await plantDataDao.FetchPlantDataAsync(userId);
    }

}