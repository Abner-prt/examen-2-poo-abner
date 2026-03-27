using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Examen2Api.Entities
{
    public class Planilla
    {
        public int Id { get; set; }
        
        [Required]
        public string Periodo { get; set; } = string.Empty;
        
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime FechaPago { get; set; }
        public string Estado { get; set; } = "Pendiente"; // "Pendiente", "Pagada", "Anulada"

        public ICollection<DetallePlanilla> Detalles { get; set; } = new List<DetallePlanilla>();
    }
}
