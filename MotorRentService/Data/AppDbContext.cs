using Microsoft.EntityFrameworkCore;
using MotorRentService.Models;

namespace MotorRentService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
    {

    }

    public DbSet<Motorcycle> Motorcycles { get; set; }
    public DbSet<DeliveryPerson> DeliveryPerson { get; set; }
    public DbSet<Rental> Rental { get; set; }
    public DbSet<RentalPlan> RentalPlan { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Motorcycle>().ToTable("Motorcycles");
        modelBuilder.Entity<DeliveryPerson>().ToTable("DeliveryPerson");
        modelBuilder.Entity<Rental>().ToTable("Rental");
        modelBuilder.Entity<RentalPlan>().ToTable("RentalPlan");
        modelBuilder.Entity<Notification>().ToTable("Notification");
    }

}
