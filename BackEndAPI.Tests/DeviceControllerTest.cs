using System.Net;
using System.Text;
using BackEndAPI.Tests;
using Domain.Model;
using Newtonsoft.Json;
using NUnit.Framework;

[TestFixture]
public class DeviceControllerTests : DatabaseTestFixture
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

    /*
    [Test]
    public async Task CreateAsync_ShouldReturnCreatedStatus()
    {
        // Arrange
        var user = new User()
        {
            UserId = 1,
            Username = "Test",
            Password = "Test"
        };

        var preset = new PlantPreset()
        {
            PresetId = 1,
            UserId = 1, 
            Name = "TestPreset",
            Humidity = 50,
            UVLight = 500,
            Moisture = 30,
            Temperature = 25,
        };

        var plant = new Plant
        {
            PlantId = 1,
            User = user,
            Location = "testLocation",
            Name = "testPreset",
            PlantPreset = preset,
            IconId = 1
        };

        Context.Users.Add(user);
        Context.Presets.Add(preset);
        Context.Plants.Add(plant);
        await Context.SaveChangesAsync();

        // Arrange
        var newDevice = new Device()
        {
            DeviceId = 1,
            Plant = plant,
            Status = true
        };

        var content = new StringContent(JsonConvert.SerializeObject(newDevice), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("Device/registerDevice", content);

        // Check for detailed error information in case of failure
        if (response.StatusCode != HttpStatusCode.Created)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Content: {responseContent}");
            // Attempt to access the inner exception details
           
        }

        // Assert
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        
        ClearDatabase();
    }
    */

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
        ClearDatabase();
    }
}