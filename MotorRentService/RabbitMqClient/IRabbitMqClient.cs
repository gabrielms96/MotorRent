using MotorRentService.Dtos;

namespace MotorRentService.RabbitMqClient
{
    public interface IRabbitMqClient
    {
        void SendNewNotification(NotificationDto notificationReadDto);
    }
}
