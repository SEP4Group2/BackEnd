using BackEndAPI.Controllers;
using BackEndAPI.Tests;
using DataAccess.DAOInterfaces;
using DataAccess.DAOs;
using Domain.DTOs;
using Domain.Model;
using Logic.Implementations;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;


[TestFixture]
public class DeviceControllerTest : DatabaseTestFixture
{
    
    private IDeviceManager deviceManager;
    private IDeviceDAO deviceDao;
    private DeviceController controller;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        deviceDao = new DeviceDAO(Context);
        deviceManager = new DeviceManagerImpl(deviceDao);
        controller = new DeviceController(deviceManager);
    }

    
    [Test]
    public async Task CreateAsync_ShouldReturnCreatedStatus()
    {
        ClearDatabase();
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
        var newDevice = new DeviceRegistrationDTO()
        {
            DeviceId = 1,
        };

        // Act
        var result = await controller.CreateAsync(newDevice.DeviceId);

        // Assert
        var createdResult = result.Result;
        Assert.That(createdResult, Is.TypeOf<CreatedResult>());
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
        var result = await controller.GetDeviceId(device.DeviceId);

        // Assert
        var createdResult = result.Result;
        Assert.That(createdResult, Is.TypeOf<OkObjectResult>());
    }
}