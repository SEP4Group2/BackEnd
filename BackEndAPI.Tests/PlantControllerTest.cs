using System.Net;
using System.Text;
using BackEndAPI.Controllers;
using Domain.DTOs;
using Domain.Model;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BackEndAPI.Tests;

[TestFixture]
public class PlantControllerIntegrationTests : DatabaseTestFixture
{
    private HttpClient _client;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {

        _client = new HttpClient()
        {
            BaseAddress = new Uri("http://localhost:5000")
        };
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
            
        Context.Devices.Add(device);
        Context.Users.Add(user1);
        Context.Presets.Add(plantPreset);
        await Context.SaveChangesAsync();
        
            
        var plantCreationDto = new PlantCreationDTO
        {
            Location = "TestLocation",
            PlantPresetId = 1, // Assuming a valid preset ID for testing
            UserId = 1, // Assuming a valid user ID for testing
            DeviceId = 1, // Assuming a valid device ID for testing
            Name = "TestPlant",
            IconId = 1
        };
        
        var content = new StringContent(JsonConvert.SerializeObject(plantCreationDto), Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync($"Plant/createPlant", content);
        
        // Assert
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        
    }

    [Test]
    public async Task GetPlantAsync_ShouldReachController()
    {
        // Act
        var response = await _client.GetAsync($"Plant/{1}");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Test]
    public async Task EditAsync_ShouldReachController()
    {
            
        // Arrange
        var plantToUpdate = new Plant()
        {
            PlantId = 1,
            Name = "MyPlant",
            Location = "Room"
        };

        var content = new StringContent(JsonConvert.SerializeObject(plantToUpdate), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PatchAsync("Plant", content);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Test]
    public async Task DeletePlant_ShouldReturnOk()
    {
        // Arrange
        int plantIdToDelete = 1; // Replace with the actual plant ID

        var mockPlantManager = new Mock<IPlantManager>();
        mockPlantManager.Setup(manager => manager.RemoveAsync(It.IsAny<int>()))
            .Returns(Task.CompletedTask);

        var plantController = new PlantController(mockPlantManager.Object);

        // Act
        var result = await plantController.DeletePlantAsync(plantIdToDelete);

        // Assert
        var statusCodeResult = (StatusCodeResult)result;
        Assert.AreEqual(200, statusCodeResult.StatusCode);
    }

}


