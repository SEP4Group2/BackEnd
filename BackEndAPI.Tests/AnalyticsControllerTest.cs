
using System.Net;
using BackEndAPI.Tests;
using Domain.DTOs;
using Domain.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;

[TestFixture]
public class AnalyticsControllerIntegrationTests : DatabaseTestFixture
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
    public async Task GetAnalyticsData_ShouldReturnOkStatus()
    {
        // Arrange
        ClearDatabase();
        // Add a sample plant and associated data to the database
        var plant = new Plant
        {
            Name = "Plant",
            Location = "Kitchen",
            IconId = 1,
            User = new User()
            {
                UserId = 1,
              Username  = "Anushri",
              Password = "Gupta"
            },
            PlantPreset = new PlantPreset()
            {
                Humidity = 20,
                Moisture = 20,
                Name = "Preset",
                Temperature = 20,
                UVLight = 200,
                UserId = 1
            }
        };
        var device = new Device()
        {
            Plant = plant
        };
        
        
        var data1 = new PlantData
            { TimeStamp = "2022-11-01 12:00:00", Humidity = 50, Temperature = 25, Moisture = 30, UVLight = 100, PlantDevice = device};
        var data2 = new PlantData
            { TimeStamp = "2022-12-02 14:30:00", Humidity = 45, Temperature = 24, Moisture = 28, UVLight = 90, PlantDevice = device };
        var data3 = new PlantData
            { TimeStamp = "2022-12-05 10:45:00", Humidity = 55, Temperature = 26, Moisture = 32, UVLight = 110 , PlantDevice = device};

        Context.PlantData.Add(data1);
        Context.PlantData.Add(data2);
        Context.PlantData.Add(data3);
        Context.Plants.Add(plant);
        Context.Devices.Add(device);
        await Context.SaveChangesAsync();

       
        var response = await _client.GetAsync($"/Analytics/{plant.PlantId}");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
}