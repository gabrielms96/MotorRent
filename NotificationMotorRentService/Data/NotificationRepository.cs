using System;
using System.Collections.Generic;
using System.Linq;
using NotificationMotorRentService.Models;

namespace NotificationMotorRentService.Data
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public void StoreNotification(Notification notification)
        {
            _context.Notification.Add(notification);
            SaveChanges();
        }

        public IEnumerable<Notification> GetAllNotifications()
        {
            return _context.Notification;
        }

        public IEnumerable<Notification> GetNotificationDeliveryPersonId(string deliveryPersonId)
        {
            return _context.Notification.Where(n => n.DeliveryPersonId == deliveryPersonId);
        }

        public IEnumerable<Notification> GetNotificationDeliveryMotorcycleId(string motorcycleId)
        {
            return _context.Notification.Where(n => n.MotorcycleId == motorcycleId);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}