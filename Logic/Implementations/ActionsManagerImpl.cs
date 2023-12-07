using System.Net.Sockets;
using Logic.Interfaces;

namespace Logic.Implementations;

public class ActionsManagerImpl : IActionsManager
{
    public HttpClient GetHttpClient()
    {
        return new HttpClient();
    }

}