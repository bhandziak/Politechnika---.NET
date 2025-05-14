using Microsoft.EntityFrameworkCore;

namespace CarWorkshopProjekt.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<ServiceOrder> ServiceOrders { get; set; }
        public DbSet<ServiceTask> ServiceTasks { get; set; }
        public DbSet<UsedPart> UsedParts { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TODO zależności kluczy i relacje
        }
    }
}
