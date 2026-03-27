using Microsoft.EntityFrameworkCore;
using Examen2Api.Entities;

namespace Examen2Api.Data
{
    public class PayrollDbContext : DbContext
    {
        public PayrollDbContext(DbContextOptions<PayrollDbContext> options) : base(options)
        {
        }
        public DbSet<EmployeeEntity> Empleados { get; set; }
        public DbSet<Sheet> Planillas { get; set; }
        public DbSet<SheetDetail> DetallesPlanilla { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmployeeEntity>()
                .HasIndex(e => e.Document)
                .IsUnique();

            
            modelBuilder.Entity<SheetDetail>()
                .HasOne(d => d.Payroll)
                .WithMany(p => p.Details)
                .HasForeignKey(d => d.PayrollId);
        }
    }
}
