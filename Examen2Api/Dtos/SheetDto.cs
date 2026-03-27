using System;
using System.ComponentModel.DataAnnotations;

namespace Examen2Api.Dtos
{
    public class PlanillaDto
    {
        public int Id { get; set; }
        public string Period { get; set; } 
        public DateTime CreationDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public string State { get; set; } 
    }

    public class PlanillaCreateDto
    {
        [Required]
        public string Period { get; set; } 
        public DateTime PaymentDate { get; set; }
    }

    public class PlanillaEstadoUpdateDto
    {
        [Required]
        public string Estado { get; set; } 
    }
}
