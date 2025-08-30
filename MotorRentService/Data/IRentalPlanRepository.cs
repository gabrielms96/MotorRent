using MotorRentService.Models;

namespace MotorRentService.Data
{
    public interface IRentalPlanRepository
    {
        public IEnumerable<RentalPlan> GetAllRentalPlans();
        public IEnumerable<RentalPlan> GetRentalPlanByName(string name);
        public void UpdateRentalPlan(RentalPlan rentalPlan);
        public void CreateRentalPlan(RentalPlan rentalPlan);
        public void DeleteRentalPlan(RentalPlan rentalPlan);
        public void SaveChanges();
    }
}
