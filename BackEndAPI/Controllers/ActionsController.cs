using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackEndAPI.Controllers;


[ApiController]
[Route("Actions")]
public class ActionsController : ControllerBase
{
    private IActionsSender _actionsSender;

    public ActionsController(IActionsSender actionsManager)
    {
        _actionsSender = actionsManager;
    }
    
    [HttpPost]
    [Route("waterPlant/{deviceId:int}")]
    public async Task<IActionResult> WaterPlant([FromRoute] int deviceId)
    {
        try
        {
            string response = await _actionsSender.SendWaterPlantAction(deviceId);
            return Ok(response);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}

    
    