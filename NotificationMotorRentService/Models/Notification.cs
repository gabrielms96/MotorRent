using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NotificationMotorRentService.Models
{
    public class Notification
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string DeliveryPersonId { get; set; }

        [Required]
        public string MotorcycleId { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public bool IsProcessed { get; set; }
    }
}