using System.Net.Sockets;

namespace Logic.Interfaces;

public interface IActionsManager
{
    NetworkStream tcpClientStream();
}