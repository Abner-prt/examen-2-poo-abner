using System.ComponentModel.DataAnnotations;

namespace Examen2Api.Dtos
{
    public class DetallePlanillaDto
    {
        public int Id { get; set; }
        public int PlanillaId { get; set; }
        public int EmpleadoId { get; set; }
        public string NombreEmpleado { get; set; } = string.Empty;

        public decimal SalarioBase { get; set; }
        public decimal HorasExtra { get; set; }
        public decimal MontoHorasExtra { get; set; }
        public decimal Bonificaciones { get; set; }
        public decimal Deducciones { get; set; }
        public decimal SalarioNeto { get; set; }
        public string Comentarios { get; set; } = string.Empty;
    }

    public class DetallePlanillaCreateUpdateDto
    {
        [Required]
        public int PlanillaId { get; set; }
        
        [Required]
        public int EmpleadoId { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Las horas extra deben ser un número positivo.")]
        public decimal HorasExtra { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "El monto por horas extra debe ser un número positivo.")]
        public decimal MontoHorasExtra { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Las bonificaciones deben ser un número positivo.")]
        public decimal Bonificaciones { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Las deducciones deben ser un número positivo.")]
        public decimal Deducciones { get; set; }
        
        public string Comentarios { get; set; } = string.Empty;
    }
}
