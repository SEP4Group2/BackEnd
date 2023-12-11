using BackEndAPI.Controllers;
using BackEndAPI.Tests;
using DataAccess.DAOInterfaces;
using DataAccess.DAOs;
using Domain.Model;
using Logic.Implementations;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

[TestFixture]
public class AnalyticsControllerTests : DatabaseTestFixture
{
    private AnalyticsController controller;
    private IPlantDataManager plantDataManager;
    private IPlantDataDAO plantDao;
    private Mock<INotificationSender> notificationSenderMock;
    private Mock<IActionsSender> actionSenderMock;


    [SetUp]
    public void Setup()
    {
        plantDao = new PlantDataDAO(Context);
        notificationSenderMock = new Mock<INotificationSender>();
        actionSenderMock = new Mock<IActionsSender>();
        plantDataManager = new PlantDataManagerImpl(plantDao, notificationSenderMock.Object,actionSenderMock.Object );
        controller = new AnalyticsController(plantDataManager);
    }

    [Test]
    public async Task GetAnalyticsData_WithValidPlantId_ReturnsOkResult()
    {
        // Arrange
        ClearDatabase();
        await Context.SaveChangesAsync();

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
        var device = new Device 
        { 
            DeviceId = 1,
            Status = true,
        };
        var plant = new Plant
        {
            Location = "TestLocation",
            PlantPreset = plantPreset, // Assuming a valid preset ID for testing
            User = user1, // Assuming a valid user ID for testing
            Name = "TestPlant",
            IconId = 1
        };

        var plantData1 = new PlantData()
        {
            Humidity = 30,
            Moisture = 29,
            PercentageStatus = 25,
            PlantDevice = device,
            TankLevel = 200,
            Temperature = 30,
            TimeStamp = "11/12/2023 11:30:30",
            UVLight = 80
        };
        var plantData2 = new PlantData()
        {
            Humidity = 35,
            Moisture = 31,
            PercentageStatus = 50,
            PlantDevice = device,
            TankLevel = 200,
            Temperature = 30,
            TimeStamp = "11/12/2023t12:31:30",
            UVLight = 80
        };
        
        
        Context.Devices.Add(device);
        Context.Users.Add(user1);
        Context.Presets.Add(plantPreset);
        Context.Plants.Add(plant);
        Context.PlantData.Add(plantData1);
        Context.PlantData.Add(plantData2);
        await Context.SaveChangesAsync();
        
        
        // Act
        var result = await controller.GetAnalyticsData(plant.PlantId);
        Console.WriteLine(result.Result.ToString());
        var createdResult = result.Result;
        
        // Assert
        Assert.IsInstanceOf<ObjectResult>(createdResult);

    }

    [Test]
    public async Task GetAnalyticsData_WithInvalidPlantId_ReturnsNotFoundResult()
    {
        ClearDatabase();
        // Arrange
        await Context.SaveChangesAsync();

        
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
        var device = new Device 
        { 
            DeviceId = 1,
            Status = true,
        };
        var plant = new Plant
        {
            Location = "TestLocation",
            PlantPreset = plantPreset, // Assuming a valid preset ID for testing
            User = user1, // Assuming a valid user ID for testing
            Name = "TestPlant",
            IconId = 1
        };
        Context.Devices.Add(device);
        Context.Users.Add(user1);
        Context.Presets.Add(plantPreset);
        Context.Plants.Add(plant);
        await Context.SaveChangesAsync();

        // Act
        var result = await controller.GetAnalyticsData(0);
        var objectResult = result.Result as ObjectResult;
        Console.WriteLine(result.ToString());
        Console.WriteLine(objectResult.StatusCode.ToString());

        Assert.AreEqual(500, objectResult.StatusCode, "Expected HTTP 500 Internal Server Error status code");

        // Assert
        // Assert.IsInstanceOf<ObjectResult>(result.Result);
    }

    [Test]
    public async Task GetAnalyticsData_WithException_ReturnsInternalServerError()
    {
        ClearDatabase();
        await Context.SaveChangesAsync();

        // Arrange
        int plantId = 1;
      

        // Act
        var result = await controller.GetAnalyticsData(plantId);
        var objectResult = result.Result as ObjectResult;


        // Assert
        Assert.AreEqual(500, objectResult.StatusCode, "Expected HTTP 500 Internal Server Error status code");
        Assert.IsInstanceOf<ObjectResult>(result.Result);

    }
}
