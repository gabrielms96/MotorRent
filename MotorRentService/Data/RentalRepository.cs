using MotorRentService.Models;

namespace MotorRentService.Data
{
    public class RentalRepository : IRentalRepository
    {
        private AppDbContext _context;


        public RentalRepository(AppDbContext context)
        {
            _context = context;
        }
        public void CreateRental(Rental rental)
        {
            _context.Rental.Add(rental);
            SaveChanges();
        }

        public Rental GetRentalById(string id)
        {
            var rental = _context.Rental.FirstOrDefault(r => r.Id == id);

            if (rental == null)
                throw new ArgumentNullException(nameof(rental));  
            return rental;

        }

        public void ReturnRental(Rental rental)
        {
            _context.Rental.Update(rental);
            SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
