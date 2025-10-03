#region Mainetence
/*
Comment: Created Rental Calculation Service.
Updated: 10/03/2025
Author:  Gabriel MS
*/
#endregion

using MotorRentService.Data;
using MotorRentService.Models;
using Microsoft.EntityFrameworkCore;

namespace MotorRentService.Services;

/// <summary>
/// Service to handle rental calculations
/// </summary>
public class RentalCalculationService : IRentalCalculationService
{
    private readonly AppDbContext _context;
    private readonly ILogger<RentalCalculationService> _logger;

    public RentalCalculationService(AppDbContext context, ILogger<RentalCalculationService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Calcula o valor total da locação baseado no plano
    /// </summary>
    /// <param name="planId">Plan ID</param>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <returns>Total value</returns>
    public async Task<decimal> CalculateTotalCostAsync(int planId, DateTime startDate, DateTime endDate)
    {
        try
        {
            _logger.LogInformation("Calculating total cost for plan: {PlanId} from {StartDate} to {EndDate}", 
                planId, startDate, endDate);

            var plan = await GetRentalPlanAsync(planId);
            if (plan == null)
            {
                _logger.LogWarning("Rental plan not found: {PlanId}", planId);
                return 0;
            }

            var days = (endDate - startDate).Days;
            if (days <= 0)
            {
                _logger.LogWarning("Invalid date range: {StartDate} to {EndDate}", startDate, endDate);
                return 0;
            }

            var totalCost = days * plan.DailyRate;
            
            _logger.LogInformation("Total cost calculated: {TotalCost} for {Days} days at {DailyRate} per day", 
                totalCost, days, plan.DailyRate);

            return totalCost;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating total cost for plan: {PlanId}", planId);
            return 0;
        }
    }

    /// <summary>
    /// Calculates fine for early return
    /// 7 days 20% and 15 days 40% of unused days
    /// </summary>
    /// <param name="rental">Rental data</param>
    /// <param name="returnDate">Return Date</param>
    /// <returns>Fine amount</returns>
    public async Task<decimal> CalculateEarlyReturnFineAsync(Rental rental, DateTime returnDate)
    {
        try
        {
            _logger.LogInformation("Calculating early return fine for rental: {RentalId}", rental.Id);

            if (returnDate >= rental.ExpectedEndDate)
            {
                _logger.LogInformation("No early return fine - returned on or after expected date");
                return 0;
            }

            var plan = await GetRentalPlanAsync(rental.IdPlan);
            if (plan == null)
            {
                _logger.LogWarning("Rental plan not found: {PlanId}", rental.IdPlan);
                return 0;
            }

            var unusedDays = (rental.ExpectedEndDate - returnDate).Days;
            var fineAmount = 0m;
            if (plan.DurationDays == 7)
                fineAmount = unusedDays * rental.DailyRate * 0.2m;
            if (plan.DurationDays == 15)
                fineAmount = unusedDays * rental.DailyRate * 0.4m;

            _logger.LogInformation("Early return fine calculated: {FineAmount} for {UnusedDays} unused days", 
                fineAmount, unusedDays);

            return fineAmount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating early return fine for rental: {RentalId}", rental.Id);
            return 0;
        }
    }

    /// <summary>
    /// Calculates fine for late return
    /// R$ 50,00 per day late
    /// </summary>
    /// <param name="rental">Rental data</param>
    /// <param name="returnDate">Return Date</param>
    /// <returns>Fine amount</returns>
    public async Task<decimal> CalculateLateReturnFineAsync(Rental rental, DateTime returnDate)
    {
        try
        {
            _logger.LogInformation("Calculating late return fine for rental: {RentalId}", rental.Id);

            if (returnDate <= rental.ExpectedEndDate)
            {
                _logger.LogInformation("No late return fine - returned on or before expected date");
                return 0;
            }

            var lateDays = (returnDate - rental.ExpectedEndDate).Days;
            var fineAmount = lateDays * 50.00m;

            _logger.LogInformation("Late return fine calculated: {FineAmount} for {LateDays} late days", 
                fineAmount, lateDays);

            return fineAmount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating late return fine for rental: {RentalId}", rental.Id);
            return 0;
        }
    }

    /// <summary>
    /// Final value calculation including fines
    /// </summary>
    /// <param name="rental">Rental data</param>
    /// <param name="returnDate">Return Date</param>
    /// <returns>Final value</returns>
    public async Task<decimal> CalculateFinalValueAsync(Rental rental, DateTime returnDate)
    {
        try
        {
            _logger.LogInformation("Calculating final value for rental: {RentalId}", rental.Id);

            var totalCost = rental.TotalCost;
            var earlyReturnFine = await CalculateEarlyReturnFineAsync(rental, returnDate);
            var lateReturnFine = await CalculateLateReturnFineAsync(rental, returnDate);

            var finalValue = totalCost + earlyReturnFine + lateReturnFine;

            _logger.LogInformation("Final value calculated: {FinalValue} (Cost: {TotalCost}, Early Fine: {EarlyFine}, Late Fine: {LateFine})", 
                finalValue, totalCost, earlyReturnFine, lateReturnFine);

            return finalValue;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating final value for rental: {RentalId}", rental.Id);
            return rental.TotalCost;
        }
    }

    /// <summary>
    /// Get rental plan data by ID
    /// </summary>
    /// <param name="planId">ID plan</param>
    /// <returns>Plan data</returns>
    public async Task<RentalPlan?> GetRentalPlanAsync(int planId)
    {
        try
        {
            return await _context.RentalPlan
                .FirstOrDefaultAsync(p => p.Id == planId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting rental plan: {PlanId}", planId);
            return null;
        }
    }

    /// <summary>
    /// Validate rental dates
    /// </summary>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <returns>True or False</returns>
    public bool ValidateRentalDates(DateTime startDate, DateTime endDate)
    {
        var today = DateTime.Today;
        
        if (startDate.Date < today)
        {
            _logger.LogWarning("Start date {StartDate} is in the past", startDate);
            return false;
        }

        if (endDate <= startDate)
        {
            _logger.LogWarning("End date {EndDate} must be after start date {StartDate}", endDate, startDate);
            return false;
        }

        if (startDate > today.AddYears(1))
        {
            _logger.LogWarning("Start date {StartDate} is too far in the future", startDate);
            return false;
        }

        return true;
    }

    /// <summary>
    /// Calculates end date based on plan and start date
    /// </summary>
    /// <param name="planId">Plan ID</param>
    /// <param name="startDate">Start date</param>
    /// <returns>End date</returns>
    public async Task<DateTime?> CalculateEndDateAsync(int planId, DateTime startDate)
    {
        try
        {
            var plan = await GetRentalPlanAsync(planId);
            if (plan == null)
            {
                _logger.LogWarning("Rental plan not found: {PlanId}", planId);
                return null;
            }

            return startDate.AddDays(plan.DurationDays);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating end date for plan: {PlanId}", planId);
            return null;
        }
    }
}

