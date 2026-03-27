using Microsoft.EntityFrameworkCore;
using Examen2Api.Entities;

namespace Examen2Api.Data
{
    public class PayrollDbContext : DbContext
    {
        public PayrollDbContext(DbContextOptions<PayrollDbContext> options) : base(options)
        {
        }

        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Planilla> Planillas { get; set; }
        public DbSet<DetallePlanilla> DetallesPlanilla { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure unique index for Documento
            modelBuilder.Entity<Empleado>()
                .HasIndex(e => e.Documento)
                .IsUnique();

            // Configure relationship between Planilla and DetallePlanilla
            modelBuilder.Entity<DetallePlanilla>()
                .HasOne(d => d.Planilla)
                .WithMany(p => p.Detalles)
                .HasForeignKey(d => d.PlanillaId);
        }
    }
}
