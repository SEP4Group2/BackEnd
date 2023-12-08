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
public class DeviceControllerIntegrationTests : DatabaseTestFixture
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
    public async Task CreateAsync_ShouldReturnCreatedStatus()
    {
        // Arrange
        ClearDatabase();
        var deviceRegistrationDto = new DeviceRegistrationDTO();
        
        var content = new StringContent(JsonConvert.SerializeObject(deviceRegistrationDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/Device/registerDevice", content);

        // Assert
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
    }

    [Test]
    public async Task GetDeviceId_ShouldReturnOkStatus()
    {
        // Arrange
        ClearDatabase();
        // Add a sample device to the database
        var device = new Device
        {
            Status = true
        };
        Context.Devices.Add(device);
        await Context.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"/Device/{device.DeviceId}");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
}