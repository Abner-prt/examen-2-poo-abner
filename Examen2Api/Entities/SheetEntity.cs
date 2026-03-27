using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Examen2Api.Entities
{
    public class Sheet
    {
        public int Id { get; set; }
        
        [Required]
        public string Period { get; set; } = string.Empty;
        
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime PaymentDate { get; set; }
        public string State { get; set; } = "Pendiente"; // "Pendiente", "Pagada", "Anulada"

        public ICollection<SheetDetail> Details { get; set; } = new List<SheetDetail>();
    }
}
