using BackEndAPI.Controllers;
using DataAccess.DAOs;
using Domain.DTOs;
using Domain.Model;
using Logic.Implementations;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;


[TestFixture]
public class PlantDataControllerTests : DatabaseTestFixture
{

    private PlantDataManagerImpl plantDataManager;
    private Mock<INotificationSender> notificationSenderMock;
    private PlantDataDAO plantDataDao;
    private PlantDataController controller;
    private Mock<IActionsSender> actionMock;
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        plantDataDao = new PlantDataDAO(Context);
        notificationSenderMock = new Mock<INotificationSender>();
        actionMock = new Mock<IActionsSender>();
        plantDataManager = new PlantDataManagerImpl(plantDataDao, notificationSenderMock.Object, actionMock.Object);
        controller = new PlantDataController(plantDataManager);
    }

    [Test]
    public async Task CreateAsync_ShouldReturnCreatedStatus()
    {
                 
                 ClearDatabase();
                 
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
               
                 var plantData1 = new PlantData()
                 {
                     PlantDevice = device, // Assuming a valid device ID for testing
                     Humidity = 50,
                     Temperature = 25,
                     Moisture = 300,
                     UVLight = 500,
                     TimeStamp = "11/02/2023,11:11:03",
                     TankLevel = 75
                 };
                 var plantData2 = new PlantData()
                 {

                     PlantDevice = device, // Assuming a valid device ID for testing
                     Humidity = 50,
                     Temperature = 25,
                     Moisture = 300,
                     UVLight = 501,
                     TimeStamp = "11/02/2023,11:11:05",
                     TankLevel = 75
                 };
                 Context.Devices.Add(device);
                 Context.Users.Add(user1);
                 Context.Presets.Add(plantPreset);
                 Context.Plants.Add(plant);
                 Context.PlantData.Add(plantData1);
                 Context.PlantData.Add(plantData2);
                 
                 await Context.SaveChangesAsync();
               
                 var dto1 = new PlantDataCreationDTO(){
                     
                     DeviceId = plantData1.PlantDevice.DeviceId, // Assuming a valid device ID for testing
                     Humidity = plantData1.Humidity,
                     Temperature = plantData1.Temperature,
                     Moisture = plantData1.Moisture,
                     UVLight = plantData1.UVLight,
                     TimeStamp = plantData1.TimeStamp,
                     TankLevel = plantData1.TankLevel
                 };
                 
                 var dto2= new PlantDataCreationDTO(){
                     
                     DeviceId = plantData2.PlantDevice.DeviceId, // Assuming a valid device ID for testing
                     Humidity = plantData2.Humidity,
                     Temperature = plantData2.Temperature,
                     Moisture = plantData2.Moisture,
                     UVLight = plantData2.UVLight,
                     TimeStamp = plantData2.TimeStamp,
                     TankLevel = plantData2.TankLevel
                 };

                 List<PlantDataCreationDTO> dataList = new List<PlantDataCreationDTO>();
                 dataList.Add(dto1);
                 dataList.Add(dto2);

                 var list = new PlantDataCreationListDTO()
                 {
                     PlantDataApi = dataList
                 };
                 
                 // Act
                 var result = await controller.CreateAsync(list);
                 
                 // Assert
                 Assert.NotNull(result);
                 var createdResult = result.Result;
                 Assert.That(createdResult, Is.TypeOf<OkResult>());
                 
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