using System;
using System.ComponentModel.DataAnnotations;

namespace Examen2Api.Entities
{
    public class Empleado
    {
        public int Id { get; set; }
        
        [Required]
        public string Nombre { get; set; } = string.Empty;
        
        [Required]
        public string Apellido { get; set; } = string.Empty;
        
        [Required]
        public string Documento { get; set; } = string.Empty;
        
        public DateTime FechaContratacion { get; set; }
        public string Departamento { get; set; } = string.Empty;
        public string PuestoTrabajo { get; set; } = string.Empty;
        public decimal SalarioBase { get; set; }
        public bool Activo { get; set; } = true;
    }
}
