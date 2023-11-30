using System.Net.Http.Json;
using Domain.DTOs;
using Logic.Interfaces;

namespace Logic.Implementations;

public class HttpClientNotificationSender : INotificationSender
{
    private readonly HttpClient _httpClient;

    public HttpClientNotificationSender()
    {
        _httpClient = new HttpClient();
    }
    
    public async Task SendNotification(NotificationRequestDTO dto)
    {
        try
        {
            await _httpClient.PostAsJsonAsync("http://notificationserver:5016/notification/send", dto);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error occurred: {e}");
        }
    }
}