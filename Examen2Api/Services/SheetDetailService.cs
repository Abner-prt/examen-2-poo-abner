using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Examen2Api.Data;
using Examen2Api.Dtos;
using Examen2Api.Entities;

namespace Examen2Api.Services
{
    public class SheetDetailService : ISheetDetailService
    {
        private readonly PayrollDbContext _context;

        public SheetDetailService(PayrollDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DetallePlanillaDto>> GetByPlanillaAsync(int planillaId)
        {
            return await _context.DetallesPlanilla
                .Include(d => d.Employee)
                .Where(d => d.PayrollId == planillaId)
                .Select(d => MapToDto(d))
                .ToListAsync();
        }
        public async Task<IEnumerable<DetallePlanillaDto>> GetByEmpleadoAsync(int empleadoId)
        {
            return await _context.DetallesPlanilla
                .Include(d => d.Employee)
                .Where(d => d.EmployeeId == empleadoId)
                .Select(d => MapToDto(d))
                .ToListAsync();
        }

        public async Task<DetallePlanillaDto> GetByIdAsync(int id)
        {
            var detalle = await _context.DetallesPlanilla
                .Include(d => d.Employee)
                .FirstOrDefaultAsync(d => d.Id == id);
            return detalle == null ? null : MapToDto(detalle);
        }

        public async Task<ApiResponse<DetallePlanillaDto>> CreateAsync(DetallePlanillaCreateUpdateDto dto)
        {
            var empleado = await _context.Empleados.FindAsync(dto.EmployeeId);
            if (empleado == null)
                return ApiResponse<DetallePlanillaDto>.FailResponse("Empleado no encontrado");

            var planilla = await _context.Planillas.FindAsync(dto.PayrollId);
            if (planilla == null)
                return ApiResponse<DetallePlanillaDto>.FailResponse("Planilla no encontrada");

            if (planilla.State == "Pagada")
                return ApiResponse<DetallePlanillaDto>.FailResponse("No se puede agregar detalles a una planilla pagada");

            var detalle = new SheetDetail
            {
                PayrollId = dto.PayrollId,
                EmployeeId = dto.EmployeeId,
                SalaryBase = empleado.SalaryBase,
                ExtraHours = dto.ExtraHours,
                ExtraHoursAmount = dto.ExtraHoursAmount,
                Bonuses = dto.Bonuses,
                Deductions = dto.Deductions,
                Comments = dto.Comments
            };

            detalle.NetSalary = CalculateNeto(detalle);

            _context.DetallesPlanilla.Add(detalle);
            await _context.SaveChangesAsync();

            // Reload to get Navigation property for mapping
            await _context.Entry(detalle).Reference(d => d.Employee).LoadAsync();

            return ApiResponse<DetallePlanillaDto>.SuccessResponse(MapToDto(detalle), "Detalle creado exitosamente");
        }

        public async Task<ApiResponse<DetallePlanillaDto>> UpdateAsync(int id, DetallePlanillaCreateUpdateDto dto)
        {
            var detalle = await _context.DetallesPlanilla.Include(d => d.Payroll).FirstOrDefaultAsync(d => d.Id == id);
            if (detalle == null)
                return ApiResponse<DetallePlanillaDto>.FailResponse("Detalle no encontrado");

            if (detalle.Payroll?.State == "Pagada")
                return ApiResponse<DetallePlanillaDto>.FailResponse("No se puede modificar detalles de una planilla pagada");

            detalle.ExtraHours = dto.ExtraHours;
            detalle.ExtraHoursAmount = dto.ExtraHoursAmount;
            detalle.Bonuses = dto.Bonuses;
            detalle.Deductions = dto.Deductions;
            detalle.Comments = dto.Comments;

            detalle.NetSalary = CalculateNeto(detalle);

            await _context.SaveChangesAsync();
            
            // Reload to get Navigation property for mapping
            await _context.Entry(detalle).Reference(d => d.Employee).LoadAsync();
            
            return ApiResponse<DetallePlanillaDto>.SuccessResponse(MapToDto(detalle), "Detalle actualizado exitosamente");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var detalle = await _context.DetallesPlanilla.Include(d => d.Payroll).FirstOrDefaultAsync(d => d.Id == id);
            if (detalle == null)
                return ApiResponse<bool>.FailResponse("Detalle no encontrado");

            if (detalle.Payroll?.State == "Pagada")
                return ApiResponse<bool>.FailResponse("No se puede eliminar detalles de una planilla pagada");

            _context.DetallesPlanilla.Remove(detalle);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Detalle eliminado exitosamente");
        }

        private static decimal CalculateNeto(SheetDetail d)
        {
            // SalarioNeto = SalarioBase + MontoHorasExtra + Bonificaciones - Deducciones
            return d.SalaryBase + d.ExtraHoursAmount + d.Bonuses - d.Deductions;
        }

        private static DetallePlanillaDto MapToDto(SheetDetail d)
        {
            return new DetallePlanillaDto
            {
                Id = d.Id,
                PlanillaId = d.PayrollId,
                EmpleadoId = d.EmployeeId,
                NombreEmpleado = d.Employee.Name + " " + d.Employee.LastName,
                SalarioBase = d.SalaryBase,
                HorasExtra = d.ExtraHours,
                MontoHorasExtra = d.ExtraHoursAmount,
                Bonificaciones = d.Bonuses,
                Deducciones = d.Deductions,
                SalarioNeto = d.NetSalary,
                Comentarios = d.Comments
            };
        }
    }
}
