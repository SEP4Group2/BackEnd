using BackEndAPI.Controllers;
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

        var newDevice = new DeviceRegistrationDTO()
        {
            DeviceId = 1,
        };

        var result = await controller.CreateAsync(newDevice.DeviceId);

        var createdResult = result.Result;
        Assert.That(createdResult, Is.TypeOf<CreatedResult>());
    }
    

    [Test]
    public async Task GetDeviceId_ShouldReturnOkStatus()
    {
        ClearDatabase();
        var device = new Device
        {
            Status = true
        };
        Context.Devices.Add(device);
        await Context.SaveChangesAsync();
        var result = await controller.GetDeviceId(device.DeviceId);

        var createdResult = result.Result;
        Assert.That(createdResult, Is.TypeOf<OkObjectResult>());
    }
     [Test]
    public async Task GetAllDeviceIdsAsync_WithDeviceIds_ShouldReturnOkWithDeviceIds()
    {
        
        ClearDatabase();
        var expectedDeviceIds = new List<int> { 3, 2, 1 };
        var device1 = new Device()
        {
            DeviceId = 1,
            Status = true
        };
        var device2 = new Device()
        {
            DeviceId = 2,
            Status = true
        };
        var device3 = new Device()
        {
            DeviceId = 3,
            Status = true
        };
        Context.Devices.Add(device1);
        Context.Devices.Add(device2);
        Context.Devices.Add(device3);
        await Context.SaveChangesAsync();
        var result = await controller.GetAllDeviceIdsAsync();

        var okObjectResult = result.Result as OkObjectResult;
        Assert.IsInstanceOf<DeviceIdsResponse>(okObjectResult.Value);
        var deviceIdsResponse = okObjectResult.Value as DeviceIdsResponse;
        Assert.AreEqual(expectedDeviceIds.ToString(), deviceIdsResponse.DeviceIds.ToString());
    }

    [Test]
    public async Task GetAllDeviceIdsAsync_WithNoDeviceIds_ShouldReturnOkWithEmptyDeviceIds()
    {
        ClearDatabase();
        var result = await controller.GetAllDeviceIdsAsync();
        var okObjectResult = result.Result as OkObjectResult;
        Assert.IsInstanceOf<DeviceIdsResponse>(okObjectResult.Value);
        var deviceIdsResponse = okObjectResult.Value as DeviceIdsResponse;
        Assert.IsEmpty(deviceIdsResponse.DeviceIds);
    }

    [Test]
    public async Task SetStatusByIdAsync_ValidDeviceStatus_ShouldReturnOk()
    {
        ClearDatabase();
        var device = new Device()
        {
            DeviceId = 10,
            Status = true
        };
        Context.Devices.Add(device);
        await Context.SaveChangesAsync();
        var deviceStatusDTO = new DeviceStatusDTO
        {
            DeviceId = device.DeviceId,
            Status = false
        };
 
        var result = await controller.SetStatusByIdAsync(deviceStatusDTO);
        var createdResult = result as OkResult;

        Assert.IsInstanceOf<OkResult>(createdResult);
    }

    [Test]
    public async Task SetStatusByIdAsync_WithException_ShouldReturnInternalServerError()
    {
        ClearDatabase();
        var deviceStatusDTO = new DeviceStatusDTO
        {
            DeviceId = 1,
            Status = true
        };

        var result = await controller.SetStatusByIdAsync(deviceStatusDTO);

        Assert.IsInstanceOf<ObjectResult>(result);
        var statusCodeResult = result as ObjectResult;
        Assert.AreEqual(500, statusCodeResult.StatusCode);
    }
}