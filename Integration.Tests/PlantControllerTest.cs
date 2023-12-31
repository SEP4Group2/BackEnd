using BackEndAPI.Controllers;
using DataAccess.DAOInterfaces;
using DataAccess.DAOs;
using Domain.DTOs;
using Domain.Model;
using Logic.Implementations;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;


[TestFixture]
public class PlantControllerTests : DatabaseTestFixture
{
    private IPlantManager plantManager;
    private IPlantDAO plantDao;
    private PlantController controller;
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        plantDao = new PlantDAO(Context);

        plantManager = new PlantManagerImpl(plantDao);
        controller = new PlantController(plantManager);
    }
    

    [Test]
    public async Task CreatePlant_ShouldReturnCreatedPlant()
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
            UserId = 1, 
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
            
        Context.Devices.Add(device);
        Context.Users.Add(user1);
        Context.Presets.Add(plantPreset);
        await Context.SaveChangesAsync();
        
            
        var plantCreationDto = new PlantCreationDTO
        {
            Location = "TestLocation",
            PlantPresetId = 1, 
            UserId = 1, 
            DeviceId = 1, 
            Name = "TestPlant",
            IconId = 1
        };
        
        var result = await controller.CreateAsync(plantCreationDto);

       var createdResult = result.Result;
        Assert.That(createdResult, Is.TypeOf<CreatedResult>());
        
    }
    [Test]
    public async Task CreateAsync_WhenCreateAsyncThrowsException_ShouldReturnInternalServerError()
    {
        ClearDatabase();
       
       
            
        var plantCreationDto = new PlantCreationDTO
        {
            Location = "TestLocation",
            PlantPresetId = 0, 
            UserId = 0, 
            DeviceId = 1, 
            Name = "TestPlant",
            IconId = 1
        };
        
        var result = await controller.CreateAsync(plantCreationDto);

        Assert.IsInstanceOf<ObjectResult>(result.Result);
        var objectResult = (ObjectResult)result.Result;

        Assert.AreEqual(500, objectResult.StatusCode);
    }

   
    [Test]
    public async Task EditAsync_ShouldReachController()
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
            UserId = 1, 
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
        Context.Devices.Add(device);
        Context.Users.Add(user1);
        Context.Presets.Add(plantPreset);
        await Context.SaveChangesAsync();

        var plantCreationDto = new Plant{
            Location = "TestLocation",
            PlantPreset = plantPreset, 
            User = user1, 
            Name = "TestPlant",
            IconId = 1
        };

        Context.Plants.Add(plantCreationDto);
        await Context.SaveChangesAsync();
        
        var plantToUpdate = new EditPlantDTO()
        {
            PlantId = 1,
            Name = "MyPlant",
            Location = "Room"
        };
        
        var result = await controller.EditAsync(plantToUpdate);

        // Assert
        var createdResult = result.Result;
        Console.WriteLine($"Result Content: {createdResult?.ToString()}");

        Assert.IsInstanceOf<ActionResult<Plant>>(result);
    }
    
    [Test]
    public async Task DeletePlant_ShouldReturnOk()
    {
        // Arrange
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
            UserId = 1, 
            Name = "TestPreset",
            Humidity = 50,
            UVLight = 500,
            Moisture = 30,
            Temperature = 25,
        };
        var plant = new Plant()
        {
            Location = "Room",
            Name = "Plant",
            PlantPreset = plantPreset,
            User = user1,
            IconId = 1
        };
        var device = new Device 
        { 
            Status = true,
            Plant = plant
        };
        
        Context.Users.Add(user1);
        Context.Presets.Add(plantPreset);
        Context.Plants.Add(plant);
        Context.Devices.Add(device);
        await Context.SaveChangesAsync();

        var plantCreationDto = new Plant{
            Location = "TestLocation",
            PlantPreset = plantPreset,
            User = user1, 
            Name = "TestPlant",
            IconId = 1
        };

        Context.Plants.Add(plantCreationDto);
        await Context.SaveChangesAsync();
        
     

        var result = await controller.DeletePlantAsync(plantCreationDto.PlantId);
        Console.WriteLine($"Actual Result Type: {result?.GetType()}");

        // Assert
        Assert.IsInstanceOf<ObjectResult>(result);
    }
    
    [Test]
    public async Task GetPlantAsync_ShouldReachController()
    {
         
         //Clear database
            ClearDatabase();
            
            var user1 = new User
            {
                UserId = 11, 
                Username = "testUser1", 
                Password = "testPassword"
            };
            
            await Context.Users.AddAsync(user1);
            var plantPreset = new PlantPreset
            {
                PresetId = 1,
                UserId = 1, 
                Name = "TestPreset",
                Humidity = 50,
                UVLight = 500,
                Moisture = 30,
                Temperature = 25,
            };
            await Context.Presets.AddAsync(plantPreset);
            var plant1 = new Plant
            {
                PlantId = 1,
                User = user1,
                Location = "testLocation",
                Name = "testPreset",
                PlantPreset = plantPreset,
                IconId = 1

            };
            await Context.Plants.AddAsync(plant1);
            var plant2 = new Plant
            {
                PlantId = 2,
                User = user1,
                Location = "testLocation",
                Name = "testPreset",
                PlantPreset = plantPreset,
                IconId = 1
            };
            await Context.Plants.AddAsync(plant2);
            var plant3 = new Plant
            {
                PlantId = 3,
                User = user1,
                Location = "testLocation",
                Name = "testPreset",
                PlantPreset = plantPreset,
                IconId = 1,
            };
            await Context.Plants.AddAsync(plant3);
            var device1 = new Device
            {
                DeviceId = 1,
                Plant = plant1,
                Status = true
            };
            await Context.Devices.AddAsync(device1);
            var device2 = new Device
            {
                DeviceId = 2,
                Plant = plant2,
                Status = true
            };
            await Context.Devices.AddAsync(device2);
            var device3 = new Device
            {
                DeviceId = 3,
                Plant = plant3,
                Status = true
            };
           
          
            await Context.Devices.AddAsync(device3);

            await Context.SaveChangesAsync();

        var result = await controller.GetAllPlantsAsync(user1.UserId);

        var createdResult = result.Result;
        Assert.That(createdResult, Is.TypeOf<OkObjectResult>());
    }
    
}


