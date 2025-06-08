using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarWorkshopProjekt.Data
{
    public class AppDbContext : IdentityDbContext<User>
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
        // public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Relacje

            //relacja UsedPart <-> Part
            modelBuilder.Entity<UsedPart>()
                    .HasOne(up => up.Part)             // UsedPart ma jedną Part
                    .WithMany(p => p.UsedParts)        // Part może mieć wiele UsedParts
                    .HasForeignKey(up => up.PartId)    // Klucz obcy w UsedPart to PartId
                    .OnDelete(DeleteBehavior.Restrict); // ZABLOKUJ usunięcie Part, jeśli ma powiązane UsedParts

            //relacja UsedPart <-> ServiceTask
            modelBuilder.Entity<UsedPart>()
                .HasOne(up => up.ServiceTask)   //jeden UsedPart może mieć jeden ServiceTask
                .WithMany(st => st.UsedParts)   //ServiceTask może mieć wiele UsedParts
                .HasForeignKey(up => up.ServiceTaskId) //Klucz obcy w UsedPart to ServiceTaskId
                .OnDelete(DeleteBehavior.Cascade);   //Przy usuwaniu ServiceTask usuwane są też powiązane UsedParts
            
            //relacja ServiceTask <-> ServiceOrder
            modelBuilder.Entity<ServiceTask>()
                .HasOne(st => st.ServiceOrder)           // każdy ServiceTask należy do jednego ServiceOrder
                .WithMany(so => so.ServiceTasks)         // jeden ServiceOrder ma wiele ServiceTasków
                .HasForeignKey(st => st.ServiceOrderId)  // klucz obcy w ServiceTask to ServiceOrderId
                .OnDelete(DeleteBehavior.Cascade);       // usunięcie ServiceOrder usuwa też powiązane ServiceTaski
            
            //relacja ServiceOrder <-> User
            modelBuilder.Entity<ServiceOrder>()
                .HasOne(so => so.User)                   // jeden ServiceOrder ma jednego Usera(mechanika)
                .WithMany(u => u.ServiceOrders)          // jeden User może mieć wiele ServiceOrders(zleceń)
                .HasForeignKey(so => so.UserId)          // klucz obcy w ServiceOrder to UserId
                .OnDelete(DeleteBehavior.Restrict);      // zapobiega przypadkowemu usunięciu Usera, gdy ma zlecenia

            //relacja ServiceOrder <-> Customer
            modelBuilder.Entity<ServiceOrder>()
                .HasOne(so => so.Customer)                    // ServiceOrder ma jednego Customer
                .WithMany(c => c.ServiceOrders)              // Customer ma wiele ServiceOrders
                .HasForeignKey(so => so.CustomerId)          // Klucz obcy w ServiceOrder to CustomerId
                .OnDelete(DeleteBehavior.Restrict);          // Nie pozwalaj usuwać klienta, jeśli ma zlecenia

            // relacja ServiceOrder <-> Vehicle
            modelBuilder.Entity<ServiceOrder>()
                .HasOne(so => so.Vehicle)                   // ServiceOrder ma jeden Vehicle
                .WithMany(v => v.ServiceOrders)             // Vehicle ma wiele ServiceOrders(zleceń)
                .HasForeignKey(so => so.VehicleId)          // Klucz obcy w ServiceOrder to VehicleId
                .OnDelete(DeleteBehavior.Restrict);         // Nie pozwól usunąć pojazdu, jeśli ma zlecenia

            // relacja Comment <-> ServiceOrder
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ServiceOrder)               // określony Comment ma jedno ServiceOrder
                .WithMany(so => so.Comments)                // ServiceOrder ma wiele Comment
                .HasForeignKey(c => c.ServiceOrderId)       // Klucz obcy w Comment to ServiceOrderId
                .OnDelete(DeleteBehavior.Cascade);          // Przy usunięciu ServiceOrder usunąć też powiązane komentarze

            // relacja Comment <-> User
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)                       // Comment ma jednego Usera
                .WithMany(u => u.Comments)                 // User ma wiele Comment
                .HasForeignKey(c => c.UserId)              // Klucz obcy w Comment to UserId
                .OnDelete(DeleteBehavior.Cascade);        // usuwanie komentarzy razem z usuwaniem usera



        }
    }
}
