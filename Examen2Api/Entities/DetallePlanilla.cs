using System.ComponentModel.DataAnnotations;

namespace Examen2Api.Entities
{
    public class DetallePlanilla
    {
        public int Id { get; set; }
        public int PlanillaId { get; set; }
        public int EmpleadoId { get; set; }

        public decimal SalarioBase { get; set; }
        public decimal HorasExtra { get; set; }
        public decimal MontoHorasExtra { get; set; }
        public decimal Bonificaciones { get; set; }
        public decimal Deducciones { get; set; }
        public decimal SalarioNeto { get; set; }
        public string Comentarios { get; set; } = string.Empty;

        // Navigation properties
        public Planilla? Planilla { get; set; }
        public Empleado? Empleado { get; set; }
    }
}
