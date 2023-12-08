using System.Net;
using System.Text;
using BackEndAPI.Tests;
using Domain.DTOs;
using Domain.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using NUnit.Framework;

[TestFixture]
public class PlantDataControllerIntegrationTests : DatabaseTestFixture
{
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
    public async Task CreateAsync_ShouldReturnCreatedStatus()
    {
        // Arrange
        var user1 = new User
        {
            UserId = 1, 
            Username = "testUser1", 
            Password = "testPassword"
        };

        var PlantPreset = new PlantPreset()
        {
            Moisture = 20,
            Humidity = 30,
            UserId = 1,
            Temperature = 20,
            UVLight = 80,
            Name = "PlantPreset",
            PresetId = 1
        };

        var Plant = new Plant()
        {
            Name = "Name",
            IconId = 1,
            PlantPreset = PlantPreset,
            User = user1,
            Location = "1",
        };
        var device = new Device 
        { 
            DeviceId = 1,
            Status = true,
            Plant = Plant
        };
        
        Context.Users.Add(user1);
        Context.Presets.Add(PlantPreset);
        Context.Plants.Add(Plant);
        Context.Devices.Add(device);
        await Context.SaveChangesAsync();
        
        
        var plantDataCreationDto = new PlantDataCreationDTO
        {
            Moisture = 20,
            DeviceId = device.DeviceId,
            Humidity = 30,
            TimeStamp = "8/12/2023 11:03:22",
            TankLevel = 100,
            Temperature = 20,
            UVLight = 80
        };
        
        var content = new StringContent(JsonConvert.SerializeObject(plantDataCreationDto), Encoding.UTF8,
            "application/json");
        Console.WriteLine($"Test Data: {await content.ReadAsStringAsync()}");

        // Act
        var response = await _client.PostAsync("/PlantData/savePlantData", content);
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
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
        var response = await _client.GetAsync($"/PlantData/fetchPlantData/{user.UserId}");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
}