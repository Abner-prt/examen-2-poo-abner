using System;
using System.ComponentModel.DataAnnotations;

namespace Examen2Api.Entities
{
    public class EmployeeEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } 
        
        [Required]
        public string LastName { get; set; } 
        
        [Required]
        public string Document { get; set; } 
        public DateTime HireDate { get; set; }
        public string Department { get; set; } 
        public string Position { get; set; } 
        public decimal SalaryBase { get; set; }
        public bool Active { get; set; } 
    }
}
