using System.Text;
using Domain.DTOs;
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
    public async Task<ActionResult<Device>> CreateAsync(DeviceRegistrationDTO device)
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


    [HttpGet]
    [Route("getAllIds")]
    public async Task<ActionResult<IEnumerable<int>>> GetAllDeviceIdsAsync()
    {
        try
        {
            IEnumerable<int> deviceIds = await deviceManager.GetAllDeviceIdsAsync();
            return Ok(deviceIds.ToList());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPatch]
    [Route("changeStatusCode")]
    public async Task<ActionResult> SetStatusByIdAsync(int id)
    {
        try
        {
            await deviceManager.SetStatusByIdAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}