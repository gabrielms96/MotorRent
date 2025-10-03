#region Mainetence
/*
Comment: Created Motorcycle availability.
Created: 10/03/2025
Author:  Gabriel MS
*/
#endregion

namespace MotorRentService.Services;

/// <summary>
/// Interface for motorcycle availability service
/// </summary>
public interface IMotorcycleAvailabilityService
{
    /// <summary>
    /// Verifys if a motorcycle is available for rent
    /// </summary>
    /// <param name="motorcycleId">Motorcycle ID</param>
    /// <returns>True if avalible and False if unavailable </returns>
    Task<bool> IsMotorcycleAvailableAsync(string motorcycleId);

    /// <summary>
    /// Verify if a motorcycle is available in a specific period
    /// </summary>
    /// <param name="motorcycleId">Motorcycle ID</param>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <returns>True if avalible and False if unavailable </returns>
    Task<bool> IsMotorcycleAvailableInPeriodAsync(string motorcycleId, DateTime startDate, DateTime endDate);

    /// <summary>
    /// Makes a motorcycle as rented
    /// </summary>
    /// <param name="motorcycleId">Motorcycle ID</param>
    /// <returns>True if avalible and False if unavailable </returns>
    Task<bool> MarkMotorcycleAsRentedAsync(string motorcycleId);

    /// <summary>
    /// Marca uma motocicleta como disponível
    /// Makes a motorcycle as available
    /// </summary>
    /// <param name="motorcycleId">Motorcycle ID</param>
    /// <returns>True se marcada com sucesso, False caso contrário</returns>
    Task<bool> MarkMotorcycleAsAvailableAsync(string motorcycleId);
}

