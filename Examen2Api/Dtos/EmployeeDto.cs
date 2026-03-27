using System;
using System.ComponentModel.DataAnnotations;

namespace Examen2Api.Dtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string LastName { get; set; } 
        public string Document { get; set; } 
        public DateTime HireDate { get; set; }
        public string Department { get; set; } 
        public string Position { get; set; } 
        public decimal SalaryBase { get; set; }
        public bool Active { get; set; }
    }

    public class EmployeeCreateUpdateDto
    {
        [Required]
        public string Name { get; set; } 
        
        [Required]
        public string LastName { get; set; } 
        
        [Required]
        public string Document { get; set; } 
        
        public DateTime HireDate { get; set; }
        public string Department { get; set; } 
        public string Position { get; set; } 
        
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be positive.")]
        public decimal SalaryBase { get; set; }
    }
}