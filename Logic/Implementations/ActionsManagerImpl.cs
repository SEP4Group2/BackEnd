using System.Net.Sockets;
using Logic.Interfaces;

namespace Logic.Implementations;

public class ActionsManagerImpl : IActionsManager
{
    public NetworkStream tcpClientStream()

    {
        string tcpServerIp = "tcpserver";
        int tcpServerPort = 23;

        TcpClient tcpClient = new TcpClient(tcpServerIp, tcpServerPort);

        return tcpClient.GetStream();

    }

}