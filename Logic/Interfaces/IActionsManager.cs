using System.Net.Sockets;

namespace Logic.Interfaces;

public interface IActionsManager
{
    public HttpClient GetHttpClient();
}