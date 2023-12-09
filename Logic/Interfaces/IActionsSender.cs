using System.Net.Sockets;

namespace Logic.Interfaces;

public interface IActionsSender
{
    public Task<string> SendWaterPlantAction(int deviceId);
}