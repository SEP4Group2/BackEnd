using System.Net.Sockets;
using System.Text;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackEndAPI.Controllers;


[ApiController]
[Route("Actions")]
public class ActionsController : ControllerBase
{
    private IActionsManager _actionsManager;
    private NetworkStream stream;

    public ActionsController(IActionsManager actionsManager)
    {
        _actionsManager = actionsManager;
        stream = actionsManager.tcpClientStream();
    }
    [HttpPost]
    [Route("waterPlant")]
    public async Task<IActionResult> SendMessage()
    {
        try
        {
                byte[] messageBytes = Encoding.UTF8.GetBytes("waterPlant"); 
                await stream.WriteAsync(messageBytes, 0, messageBytes.Length);

                // Read the response from the TCPServer
                byte[] responseBytes = new byte[4096];
                int bytesRead = await stream.ReadAsync(responseBytes, 0, responseBytes.Length);
                string response = Encoding.UTF8.GetString(responseBytes, 0, bytesRead);

                return Ok(response);
            
        }
        catch (Exception ex)
        {
            // Handle exceptions
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}

    
    