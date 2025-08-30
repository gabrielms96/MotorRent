namespace MotorRentService.Dtos
{
    public class NotificationCreateDto
    {
        public int Id { get; set; }
        public string DeliveryPersonId { get; set; }
        public string MotorcycleId { get; set; }
        public string Message { get; set; }
        public string Evento { get; set; }
        public bool IsProcessed { get; set; }
    }
}
