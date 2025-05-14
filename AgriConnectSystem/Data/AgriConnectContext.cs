using Microsoft.EntityFrameworkCore;

namespace AgriConnectSystem.Models
{
    public class AgriConnectContext : DbContext
    {
        public AgriConnectContext(DbContextOptions<AgriConnectContext> options) : base(options) { }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seeded sample users
            modelBuilder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = 1,
                    Username = "farmer1",
                    Password = "farmer123",
                    Role = "Farmer"
                },
                new AppUser
                {
                    Id = 2,
                    Username = "employee1",
                    Password = "employee123",
                    Role = "Employee"
                }
            );
        }
    }
}
