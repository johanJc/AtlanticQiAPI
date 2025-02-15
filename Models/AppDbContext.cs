using Microsoft.EntityFrameworkCore;

namespace AtlanticQiAPI.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Status> Status { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación Client - Status con la clave foránea StatusId
            modelBuilder.Entity<Client>()
                .HasOne(c => c.StatusNavigation)
                .WithMany()
                .HasForeignKey(c => c.StatusId) // Aquí usamos StatusId
                .HasConstraintName("FK_Client_Status");

            // Insertar valores iniciales en la tabla Status
            modelBuilder.Entity<Status>().HasData(
                new Status { Id = 1, Name = "Contactado" },
                new Status { Id = 2, Name = "No Contactado" }
            );
        }

    }
}
