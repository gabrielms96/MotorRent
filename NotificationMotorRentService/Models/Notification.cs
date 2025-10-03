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
        public string DeliveryPersonId { get; set; } = string.Empty;

        [Required]
        public string MotorcycleId { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        [Required]
        public string Evento { get; set; } = string.Empty;

        [Required]
        public bool IsProcessed { get; set; }
    }
}