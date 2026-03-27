using System.ComponentModel.DataAnnotations;

namespace Examen2Api.Dtos
{
    public class DetallePlanillaDto
    {
        public int Id { get; set; }
        public int PlanillaId { get; set; }
        public int EmpleadoId { get; set; }
        public string NombreEmpleado { get; set; } 

        public decimal SalarioBase { get; set; }
        public decimal HorasExtra { get; set; }
        public decimal MontoHorasExtra { get; set; }
        public decimal Bonificaciones { get; set; }
        public decimal Deducciones { get; set; }
        public decimal SalarioNeto { get; set; }
        public string Comentarios { get; set; } 
    }

    public class DetallePlanillaCreateUpdateDto
    {
        [Required]
        public int PayrollId { get; set; }
        
        [Required]
        public int EmployeeId { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Las horas extra deben ser un número positivo.")]
        public decimal ExtraHours { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "El monto por horas extra debe ser un número positivo.")]
        public decimal ExtraHoursAmount { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Las bonificaciones deben ser un número positivo.")]
        public decimal Bonuses { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Las deducciones deben ser un número positivo.")]
        public decimal Deductions { get; set; }
        
        public string Comments { get; set; } 
    }
}
