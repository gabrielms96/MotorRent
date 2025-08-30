using MotorRentService.Models;

namespace MotorRentService.Data;

    public interface IRentalRepository
    {
        void SaveChanges();

        void CreateRental(Rental rental);
        Rental GetRentalById(string id);
        void ReturnRental(Rental rental);

}
