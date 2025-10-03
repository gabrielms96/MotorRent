#region Mainetence
/*
Comment: Created Motorcycle availability.
Created: 10/03/2025
Author:  Gabriel MS
*/
#endregion

using MotorRentService.Data;
using MotorRentService.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace MotorRentService.Services;

/// <summary>
/// Service to check motorcycle availability
/// </summary>
public class MotorcycleAvailabilityService : IMotorcycleAvailabilityService
{
    private readonly AppDbContext _context;
    private readonly ILogger<MotorcycleAvailabilityService> _logger;

    public MotorcycleAvailabilityService(AppDbContext context, ILogger<MotorcycleAvailabilityService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Verify if a motorcycle is available for rent
    /// </summary>
    /// <param name="motorcycleId">Motorcycle ID</param>
    /// <returns>True if avalible and False if unavailable</returns>
    public async Task<bool> IsMotorcycleAvailableAsync(string motorcycleId)
    {
        try
        {
            _logger.LogInformation("Checking availability for motorcycle: {MotorcycleId}", motorcycleId);

            var motorcycle = await _context.Motorcycles
                .FirstOrDefaultAsync(m => m.Id == motorcycleId);

            if (motorcycle == null)
            {
                _logger.LogWarning("Motorcycle not found: {MotorcycleId}", motorcycleId);
                return false;
            }

            if (motorcycle.Status != MotorcycleStatus.Available)
            {
                _logger.LogInformation("Motorcycle {MotorcycleId} is not available. Status: {Status}", 
                    motorcycleId, motorcycle.Status);
                return false;
            }

            var hasActiveRental = await _context.Rental
                .AnyAsync(r => r.MotorcycleId == motorcycleId && 
                              r.Status == RentalStatus.Active);

            if (hasActiveRental)
            {
                _logger.LogInformation("Motorcycle {MotorcycleId} has active rental", motorcycleId);
                return false;
            }

            _logger.LogInformation("Motorcycle {MotorcycleId} is available", motorcycleId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking availability for motorcycle: {MotorcycleId}", motorcycleId);
            return false;
        }
    }

    /// <summary>
    /// Verify if a motorcycle is available in a specific period
    /// </summary>
    /// <param name="motorcycleId">Motorcycle ID</param>
    /// <param name="startDate">Start Date</param>
    /// <param name="endDate">End Date</param>
    /// <returns>True if avalible and False if unavailable </returns>
    public async Task<bool> IsMotorcycleAvailableInPeriodAsync(string motorcycleId, DateTime startDate, DateTime endDate)
    {
        try
        {
            _logger.LogInformation("Checking availability for motorcycle: {MotorcycleId} from {StartDate} to {EndDate}", 
                motorcycleId, startDate, endDate);

            if (!await IsMotorcycleAvailableAsync(motorcycleId))
                return false;

            var hasConflict = await _context.Rental
                .AnyAsync(r => r.MotorcycleId == motorcycleId &&
                              r.Status == RentalStatus.Active &&
                              ((r.StartDate <= endDate && r.ExpectedEndDate >= startDate) ||
                               (r.StartDate <= endDate && r.RealEndDate >= startDate && r.RealEndDate.HasValue)));

            if (hasConflict)
            {
                _logger.LogInformation("Motorcycle {MotorcycleId} has scheduling conflict in the requested period", motorcycleId);
                return false;
            }

            _logger.LogInformation("Motorcycle {MotorcycleId} is available in the requested period", motorcycleId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking period availability for motorcycle: {MotorcycleId}", motorcycleId);
            return false;
        }
    }

    /// <summary>
    /// Makes a motorcycle as rented
    /// </summary>
    /// <param name="motorcycleId">Motorcycle ID</param>
    /// <returns>True se disponível, False caso contrário</returns>
    public async Task<bool> MarkMotorcycleAsRentedAsync(string motorcycleId)
    {
        try
        {
            _logger.LogInformation("Marking motorcycle as rented: {MotorcycleId}", motorcycleId);

            var motorcycle = await _context.Motorcycles
                .FirstOrDefaultAsync(m => m.Id == motorcycleId);

            if (motorcycle == null)
            {
                _logger.LogWarning("Motorcycle not found: {MotorcycleId}", motorcycleId);
                return false;
            }

            motorcycle.Status = MotorcycleStatus.Rented;
            motorcycle.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Motorcycle {MotorcycleId} marked as rented", motorcycleId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking motorcycle as rented: {MotorcycleId}", motorcycleId);
            return false;
        }
    }

    /// <summary>
    /// Makes a motorcycle as rented
    /// </summary>
    /// <param name="motorcycleId">Motorcycle ID</param>
    /// <returns>True se disponível, False caso contrário</returns>
    public async Task<bool> MarkMotorcycleAsAvailableAsync(string motorcycleId)
    {
        try
        {
            _logger.LogInformation("Marking motorcycle as available: {MotorcycleId}", motorcycleId);

            var motorcycle = await _context.Motorcycles
                .FirstOrDefaultAsync(m => m.Id == motorcycleId);

            if (motorcycle == null)
            {
                _logger.LogWarning("Motorcycle not found: {MotorcycleId}", motorcycleId);
                return false;
            }

            motorcycle.Status = MotorcycleStatus.Available;
            motorcycle.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Motorcycle {MotorcycleId} marked as available", motorcycleId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking motorcycle as available: {MotorcycleId}", motorcycleId);
            return false;
        }
    }
}

