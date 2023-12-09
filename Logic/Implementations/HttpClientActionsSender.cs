using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using BackEndAPI;
using Logic.Interfaces;

namespace Logic.Implementations;

public class HttpClientActionsSender : IActionsSender

{

    private HttpClient client;
    public HttpClientActionsSender ()
    {
        this.client = new HttpClient();
    }

    public async Task<string> SendWaterPlantAction(int deviceId)
    {
        string apiUrl = "http://iotbridgeserver:5024/api/plants";

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

        return responseContent;
    }

}