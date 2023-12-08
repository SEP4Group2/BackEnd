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

namespace BackEndAPI.Tests;


[TestFixture]
public class PlantControllerIntegrationTests
{
    private TestServer _server;
    private HttpClient _client;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // Arrange
        _client = new HttpClient()
        {
            BaseAddress = new Uri("http://localhost:5000")
        };
    }

    [Test]
    public async Task CreatePlant_ShouldReturnCreatedPlant()
    {
        // Arrange
        var plantToCreate = new PlantCreationDTO
        {
            Name = "TestPlant",
            Location = "TestLocation",
            DeviceId = 1,
            IcondId = 1,
            PlantPresetId = 1,
            UserId = 1
            // Add other properties as needed
        };

        var createdPlant = new Plant
        {
            Name = "TestPlant",
            Location = "TestLocation",
            PlantPreset = new Mock<PlantPreset>().Object,
            IconId = 1,
            PlantId = 1
        };

        var mockPlantManager = new Mock<IPlantManager>();
        mockPlantManager.Setup(manager => manager.CreateAsync(It.IsAny<PlantCreationDTO>()))
            .ReturnsAsync(createdPlant);

        var plantJson = new StringContent(JsonConvert.SerializeObject(plantToCreate), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/Plant/createPlant", plantJson);

        // Assert
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        // Optionally, you can deserialize the response content and assert against the created plant
        var responseContent = await response.Content.ReadAsStringAsync();
        var createdPlantFromResponse = JsonConvert.DeserializeObject<Plant>(responseContent);

        Assert.AreEqual(createdPlant.Name, createdPlantFromResponse.Name);
        Assert.AreEqual(createdPlant.Location, createdPlantFromResponse.Location);
        // Add other assertions as needed
    }

    [Test]
    public async Task GetAllUsersAsync_ShouldReachController()
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
        var plantToUpdate = new PlantCreationDTO()
        {
            
            UserId = 6,
            Name = "MyPlant",
            PlantPresetId = 1,
            DeviceId = 1,
            IcondId = 1,
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
