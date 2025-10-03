#region Mainetence
/*
Comment: Enhanced with business rules validation.
Updated: 10/03/2025
Author:  Gabriel MS
*/
#endregion
using MotorRentService.Models;
using MotorRentService.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace MotorRentService.Data
{
    public class RentalRepository : IRentalRepository
    {
        private readonly AppDbContext _context;

        public RentalRepository(AppDbContext context)
        {
            _context = context;
        }

        public void CreateRental(Rental rental)
        {
            _context.Rental.Add(rental);
            SaveChanges();
        }

        public async Task CreateRentalAsync(Rental rental)
        {
            await _context.Rental.AddAsync(rental);
            await SaveChangesAsync();
        }

        public Rental GetRentalById(string id)
        {
            var rental = _context.Rental.FirstOrDefault(r => r.Id == id);

            if (rental == null)
                throw new ArgumentNullException(nameof(rental));  
            return rental;
        }

        public async Task<Rental?> GetRentalByIdAsync(string id)
        {
            return await _context.Rental
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public void ReturnRental(Rental rental)
        {
            _context.Rental.Update(rental);
            SaveChanges();
        }

        public async Task ReturnRentalAsync(Rental rental)
        {
            _context.Rental.Update(rental);
            await SaveChangesAsync();
        }

        public async Task<IEnumerable<Rental>> GetActiveRentalsByMotorcycleAsync(string motorcycleId)
        {
            return await _context.Rental
                .Where(r => r.MotorcycleId == motorcycleId && r.Status == RentalStatus.Active)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetRentalsByDeliveryPersonAsync(string deliveryPersonId)
        {
            return await _context.Rental
                .Where(r => r.DeliveryPersonId == deliveryPersonId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> HasActiveRentalAsync(string motorcycleId)
        {
            return await _context.Rental
                .AnyAsync(r => r.MotorcycleId == motorcycleId && r.Status == RentalStatus.Active);
        }

        public async Task<IEnumerable<Rental>> GetRentalsByStatusAsync(RentalStatus status)
        {
            return await _context.Rental
                .Where(r => r.Status == status)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> UpdateRentalStatusAsync(string rentalId, RentalStatus status)
        {
            try
            {
                var rental = await GetRentalByIdAsync(rentalId);
                if (rental == null)
                    return false;

                rental.Status = status;
                rental.UpdatedAt = DateTime.UtcNow;

                await SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
