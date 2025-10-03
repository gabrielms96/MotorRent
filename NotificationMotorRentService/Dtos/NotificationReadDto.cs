namespace NotificationMotorRentService.Dtos
{
    public class NotificationReadDto
    {
        public int Id { get; set; }
        public string DeliveryPersonId { get; set; } = string.Empty;
        public string MotorcycleId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Evento { get; set; } = string.Empty;
        public bool IsProcessed { get; set; }
    }
}