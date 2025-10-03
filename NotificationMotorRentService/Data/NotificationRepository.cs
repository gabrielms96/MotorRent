#region Mainetence
/*
Comment: Added try catch and fixed saving notification.
Updated: 10/03/2025 13:15
Author:  Gabriel MS
*/
#endregion
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
            try
            {
                _context.Notification.Add(notification);
                SaveChanges();
                
                // Log para debug
                Console.WriteLine($"Notification stored successfully: ID={notification.Id}, Message={notification.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error storing notification: {ex.Message}");
                throw;
            }
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