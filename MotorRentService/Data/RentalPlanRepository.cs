using MotorRentService.Models;

namespace MotorRentService.Data;

public class RentalPlanRepository : IRentalPlanRepository
{
    private AppDbContext _context;
    public RentalPlanRepository(AppDbContext context)
    {
        _context = context;
    }
    public void CreateRentalPlan(RentalPlan rentalPlan)
    {
        _context.RentalPlan.Add(rentalPlan);
        SaveChanges();
    }

    public void DeleteRentalPlan(RentalPlan rentalPlan)
    {
        _context.RentalPlan.Remove(rentalPlan);
        SaveChanges();
    }

    public IEnumerable<RentalPlan> GetAllRentalPlans()
    {
        return _context.RentalPlan;
    }

    public IEnumerable<RentalPlan> GetRentalPlanByName(string planName)
    {
        return _context.RentalPlan.Where(rp => rp.PlanName.Contains(planName));
    }

    public void SaveChanges()
    {
        throw new NotImplementedException();
    }

    public void UpdateRentalPlan(RentalPlan rentalPlan)
    {
        _context.RentalPlan.Update(rentalPlan);
        SaveChanges();
    }
}

