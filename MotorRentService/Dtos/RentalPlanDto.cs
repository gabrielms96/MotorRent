namespace MotorRentService.Dtos;

public class RentalPlanDto
{
    public int Id { get; set; }
    
    public string PlanName { get; set; }

    public int DurationDays { get; set; }

    public decimal DailyRate { get; set; }

    public decimal FineRate { get; set; }
}


