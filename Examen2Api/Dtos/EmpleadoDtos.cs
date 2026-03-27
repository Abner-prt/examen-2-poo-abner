using System;
using System.ComponentModel.DataAnnotations;

namespace Examen2Api.Dtos
{
    public class EmpleadoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public DateTime FechaContratacion { get; set; }
        public string Departamento { get; set; } = string.Empty;
        public string PuestoTrabajo { get; set; } = string.Empty;
        public decimal SalarioBase { get; set; }
        public bool Activo { get; set; }
    }

    public class EmpleadoCreateUpdateDto
    {
        [Required]
        public string Nombre { get; set; } = string.Empty;
        
        [Required]
        public string Apellido { get; set; } = string.Empty;
        
        [Required]
        public string Documento { get; set; } = string.Empty;
        
        public DateTime FechaContratacion { get; set; }
        public string Departamento { get; set; } = string.Empty;
        public string PuestoTrabajo { get; set; } = string.Empty;
        
        [Range(0, double.MaxValue, ErrorMessage = "El Salario Base debe ser un número positivo.")]
        public decimal SalarioBase { get; set; }
    }
}
