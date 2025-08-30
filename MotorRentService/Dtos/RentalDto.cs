namespace MotorRentService.Dtos
{
    public class RentalDto
    {
        public string Id { get; set; }
        public string DeliveryPersonId { get; set; }
        public string MotorcycleId { get; set; }
        public DateTime ActualDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public DateTime? RealEndDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int IdPlan { get; set; }
        public decimal DailyRate { get; set; }
        public decimal TotalCost { get; set; }
        public decimal? FineAmount { get; set; }
        public decimal? FinalValue { get; set; }

    }
}
