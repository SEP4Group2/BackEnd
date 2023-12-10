using System.Net;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using BackEndAPI.Controllers;
using BackEndAPI.Tests;
using DataAccess.DAOInterfaces;
using DataAccess.DAOs;
using Domain.DTOs;
using Domain.Model;
using Logic.Implementations;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

[TestFixture]
public class PlantDataControllerTests : DatabaseTestFixture
{

    private PlantDataManagerImpl plantDataManager;
    private Mock<INotificationSender> notificationSenderMock;
    private PlantDataDAO plantDataDao;
    private PlantDataController controller;
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        plantDataDao = new PlantDataDAO(Context);
        notificationSenderMock = new Mock<INotificationSender>();

        plantDataManager = new PlantDataManagerImpl(plantDataDao, notificationSenderMock.Object);
        controller = new PlantDataController(plantDataManager);
    }

    [Test]
    public async Task CreateAsync_ShouldReturnCreatedStatus()
    {
     //Clear database
                 
                 ClearDatabase();
                 
                 // Arrange
                 var user1 = new User
                 {
                     UserId = 1, 
                     Username = "testUser1", 
                     Password = "testPassword"
                 };
             
                 var plantPreset = new PlantPreset
                 {
                     PresetId = 1,
                     UserId = 1, // Assuming a valid user ID for testing
                     Name = "TestPreset",
                     Humidity = 50,
                     UVLight = 500,
                     Moisture = 30,
                     Temperature = 25,
                 };
                 
                 var plant = new Plant
                 {
                     PlantId = 1,
                     User = user1,
                     Location = "testLocation",
                     Name = "testPreset",
                     PlantPreset = plantPreset,
                     IconId = 1
                 };
                 var device = new Device 
                 { 
                     DeviceId = 1,
                     Status = true,
                     Plant = plant
                 };
                 
                 Context.Devices.Add(device);
                 Context.Users.Add(user1);
                 Context.Presets.Add(plantPreset);
                 Context.Plants.Add(plant);
                 
                 await Context.SaveChangesAsync();
               
                 var plantData = new PlantDataCreationDTO(){
                     
                     DeviceId = device.DeviceId, // Assuming a valid device ID for testing
                     Humidity = 50,
                     Temperature = 25,
                     Moisture = 30,
                     UVLight = 500,
                     TimeStamp = "11/02/2023,11:11:03",
                     TankLevel = 75
                 };

              

                 // Act
                 var result = await plantDataManager.SaveAsync(plantData);

                 // Assert
                 Assert.NotNull(result);
                 Assert.IsInstanceOf<PlantData>(result);
               
    }

    [Test]
    public async Task FetchPlantData_ShouldReturnOkStatus()
    {
        // Arrange
        ClearDatabase();
        // Add a sample user and plant data to the database
        var device = new Device()
        {
            Status = true
        };
        var user = new User()
        {
            Username = "Test",
            Password = "Test"
        };
        var plantData = new PlantData
        {
           PlantDevice = device,
           Moisture = 20,
           Humidity = 30,
           TimeStamp = "8/12/2023 11:03:22",
           TankLevel = 200,
           Temperature = 20,
           UVLight = 80
           
        };
        Context.Devices.Add(device);
        Context.PlantData.Add(plantData);
        await Context.SaveChangesAsync();

      
        // Act
        var result = await controller.FetchPlantData(user.UserId);

        // Assert
        var createdResult = result.Result;
        Assert.That(createdResult, Is.TypeOf<OkObjectResult>());
        
    }
}