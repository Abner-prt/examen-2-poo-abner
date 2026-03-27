using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Examen2Api.Dtos
{
    public class PlanillaDto
    {
        public int Id { get; set; }
        public string Periodo { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaPago { get; set; }
        public string Estado { get; set; } = string.Empty;
    }

    public class PlanillaCreateDto
    {
        [Required]
        public string Periodo { get; set; } = string.Empty;
        public DateTime FechaPago { get; set; }
    }

    public class PlanillaEstadoUpdateDto
    {
        [Required]
        public string Estado { get; set; } = string.Empty;
    }
}
