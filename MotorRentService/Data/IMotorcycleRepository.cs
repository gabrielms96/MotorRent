using MotorRentService.Dtos;
using MotorRentService.Models;

namespace MotorRentService.Data;

public interface IMotorcycleRepository
{
    Task<Motorcycle> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Motorcycle>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Motorcycle>> GetByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken = default);
    Task<bool> ExistsByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken = default);
    Task<bool> ExistsByLicensePlateAsync(string licensePlate, string excludeId, CancellationToken cancellationToken = default);
    Task AddAsync(Motorcycle motorcycle, CancellationToken cancellationToken = default);
    Task UpdateAsync(Motorcycle motorcycle, CancellationToken cancellationToken = default);
    Task DeleteAsync(Motorcycle motorcycle, CancellationToken cancellationToken = default);
    Task<IEnumerable<Motorcycle>> GetMotorcyclesById(string id, CancellationToken cancellationToken = default);

}
