using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackEndAPI.Controllers;


[ApiController]
[Route("Actions")]
public class ActionsController : ControllerBase
{
    private IActionsManager _actionsManager;
    private HttpClient client;

    public ActionsController(IActionsManager actionsManager)
    {
        _actionsManager = actionsManager;
        client = _actionsManager.GetHttpClient();
    }
    [HttpPost]
    [Route("waterPlant/{deviceId:int}")]
    public async Task<IActionResult> WaterPlant([FromRoute] int deviceId)
    {
        try
        {
            string apiUrl = "http://tcpserver";

            // Construct the JSON object
            var waterCommand = new WaterPlantCommand()
            {
                DeviceId = deviceId
            };
            string waterCommandSerialized = JsonSerializer.Serialize(waterCommand);

            var bridgeMessage = new IoTBridgeMessage()
            {
                DataType = 0,
                Data = waterCommandSerialized
            };
            string bridgeMessageSerialized = JsonSerializer.Serialize(bridgeMessage);
            
            var content = new StringContent(bridgeMessageSerialized, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(apiUrl, content);

            // Read the response from the HTTP server
            string responseContent = await response.Content.ReadAsStringAsync();

            return Ok(responseContent);
            
        }
        catch (Exception ex)
        {
            // Handle exceptions
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}

    
    