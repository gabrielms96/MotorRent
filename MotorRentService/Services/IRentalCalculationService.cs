using MotorRentService.Models;

namespace MotorRentService.Services;

/// <summary>
/// Interface for rental calculation service
/// </summary>
public interface IRentalCalculationService
{
    Task<decimal> CalculateTotalCostAsync(int planId, DateTime startDate, DateTime endDate);
    Task<decimal> CalculateEarlyReturnFineAsync(Rental rental, DateTime returnDate);
    Task<decimal> CalculateLateReturnFineAsync(Rental rental, DateTime returnDate);
    Task<decimal> CalculateFinalValueAsync(Rental rental, DateTime returnDate);
    Task<RentalPlan?> GetRentalPlanAsync(int planId);
    bool ValidateRentalDates(DateTime startDate, DateTime endDate);
    Task<DateTime?> CalculateEndDateAsync(int planId, DateTime startDate);
}

