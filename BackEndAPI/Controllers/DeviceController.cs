using Domain.Model;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackEndAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class DeviceController:ControllerBase
{
    
    private IDeviceManager deviceManager;


    public DeviceController(IDeviceManager deviceManager)
    {
        this.deviceManager = deviceManager;
    }
    
    [HttpPost]
    [Route("registerDevice")]
    public async Task<ActionResult<Device>> CreateAsync(Device device)
    {
        try
        {
            Device newDevice = await deviceManager.CreateAsync(device);
            return Created($"/file/{newDevice.DeviceId}", newDevice);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet]
    [Route("{deviceId:int}")]
    public async Task<ActionResult<int>> GetDeviceId(int deviceId)
    {

        try
        { 
            int device = await deviceManager.GetDeviceIdAsync(deviceId);
            return Ok(device);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPost]
    [Route("waterPlant/{deviceId}")]
    public async Task<ActionResult> WaterPlant([FromRoute] int deviceId)
    {
        try
        {
            int action = await deviceManager.WaterPlant(deviceId);
            return Ok(new
                {
                    Message = "Plant successfully watered",
                    DeviceId = action,
                });

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    
    
}