using NotificationMotorRentService.Models;

namespace NotificationMotorRentService.Data;

public interface INotificationRepository
{
    public void StoreNotification(Notification notification);
    public IEnumerable<Notification> GetAllNotifications();
    public IEnumerable<Notification> GetNotificationDeliveryPersonId(string deliveryPersonId);
    public IEnumerable<Notification> GetNotificationDeliveryMotorcycleId(string motorcycleId);
    public void SaveChanges();
}
