using System.ComponentModel.DataAnnotations;

namespace Examen2Api.Entities
{
    public class SheetDetail
    {
        public int Id { get; set; }
        public int PayrollId { get; set; }
        public int EmployeeId { get; set; }
        public decimal SalaryBase { get; set; }
        public decimal ExtraHours { get; set; }
        public decimal ExtraHoursAmount { get; set; }
        public decimal Bonuses { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetSalary { get; set; }
        public string Comments { get; set; }
        
        public Sheet Payroll { get; set; }
        public EmployeeEntity Employee { get; set; }
    }
}
