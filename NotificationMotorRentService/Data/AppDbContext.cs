using NotificationMotorRentService.Models;
using Microsoft.EntityFrameworkCore;

namespace NotificationMotorRentService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        public DbSet<Notification> Notification { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>().ToTable("Notification");
        }

    }
}