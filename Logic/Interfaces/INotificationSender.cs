using Domain.DTOs;

namespace Logic.Interfaces;

public interface INotificationSender
{
    Task SendNotification(NotificationRequestDTO dto);
}