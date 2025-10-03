using MotorRentService.Models;
using MotorRentService.Models.Enums;

namespace MotorRentService.Data;

public interface IRentalRepository
{
    void SaveChanges();
    Task SaveChangesAsync();
    void CreateRental(Rental rental);
    Task CreateRentalAsync(Rental rental);
    Rental GetRentalById(string id);
    Task<Rental?> GetRentalByIdAsync(string id);
    void ReturnRental(Rental rental);
    Task ReturnRentalAsync(Rental rental);
    Task<IEnumerable<Rental>> GetActiveRentalsByMotorcycleAsync(string motorcycleId);
    Task<IEnumerable<Rental>> GetRentalsByDeliveryPersonAsync(string deliveryPersonId);
    Task<bool> HasActiveRentalAsync(string motorcycleId);
    Task<IEnumerable<Rental>> GetRentalsByStatusAsync(RentalStatus status);
    Task<bool> UpdateRentalStatusAsync(string rentalId, RentalStatus status);
}
