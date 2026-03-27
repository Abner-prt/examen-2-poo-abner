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
    public class SheetService : ISheetService
    {
        private readonly PayrollDbContext _context;

        public SheetService(PayrollDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PlanillaDto>> GetAllAsync()
        {
            return await _context.Planillas
                .Select(p => MapToDto(p))
                .ToListAsync();
        }

        public async Task<PlanillaDto> GetByIdAsync(int id)
        {
            var planilla = await _context.Planillas.FindAsync(id);
            return planilla == null ? null : MapToDto(planilla);
        }

        public async Task<IEnumerable<PlanillaDto>> GetByPeriodoAsync(string periodo)
        {
            return await _context.Planillas
                .Where(p => p.Period == periodo)
                .Select(p => MapToDto(p))
                .ToListAsync();
        }

        public async Task<ApiResponse<PlanillaDto>> CreateAsync(PlanillaCreateDto dto)
        {
            var exists = await _context.Planillas.AnyAsync(p => p.Period == dto.Period);
            if (exists)
                return ApiResponse<PlanillaDto>.FailResponse($"Ya existe una planilla para el periodo {dto.Period}");

            var planilla = new Sheet
            {
                Period = dto.Period,
                CreationDate = DateTime.Now,
                PaymentDate = dto.PaymentDate,
                State = "Pendiente"
            };

            _context.Planillas.Add(planilla);
            await _context.SaveChangesAsync();

            return ApiResponse<PlanillaDto>.SuccessResponse(MapToDto(planilla), "Planilla creada exitosamente");
        }

        public async Task<ApiResponse<PlanillaDto>> UpdateAsync(int id, PlanillaCreateDto dto)
        {
            var planilla = await _context.Planillas.FindAsync(id);
            if (planilla == null)
                return ApiResponse<PlanillaDto>.FailResponse("Planilla no encontrada");

            if (planilla.State == "Pagada")
                return ApiResponse<PlanillaDto>.FailResponse("No se puede actualizar una planilla con estado 'Pagada'");

            var exists = await _context.Planillas.AnyAsync(p => p.Period == dto.Period && p.Id != id);
            if (exists)
                return ApiResponse<PlanillaDto>.FailResponse($"Ya existe otra planilla para el periodo {dto.Period}");

            planilla.Period = dto.Period;
            planilla.PaymentDate = dto.PaymentDate;

            await _context.SaveChangesAsync();
            return ApiResponse<PlanillaDto>.SuccessResponse(MapToDto(planilla), "Planilla actualizada exitosamente");
        }

        public async Task<ApiResponse<PlanillaDto>> UpdateEstadoAsync(int id, string estado)
        {
            var planilla = await _context.Planillas.FindAsync(id);
            if (planilla == null)
                return ApiResponse<PlanillaDto>.FailResponse("Planilla no encontrada");

            var validEstados = new[] { "Pendiente", "Pagada", "Anulada" };
            if (!validEstados.Contains(estado))
                return ApiResponse<PlanillaDto>.FailResponse("Estado no válido. Use 'Pendiente', 'Pagada' o 'Anulada'");

            planilla.State = estado;
            await _context.SaveChangesAsync();

            return ApiResponse<PlanillaDto>.SuccessResponse(MapToDto(planilla), "Estado de planilla actualizado exitosamente");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var planilla = await _context.Planillas.FindAsync(id);
            if (planilla == null)
                return ApiResponse<bool>.FailResponse("Planilla no encontrada");

            if (planilla.State == "Pagada")
                return ApiResponse<bool>.FailResponse("No se permite eliminar una planilla con estado 'Pagada'");

            _context.Planillas.Remove(planilla);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Planilla eliminada exitosamente");
        }

        public async Task<ApiResponse<PlanillaDto>> GenerarPlanillaAutomatica(string periodo = null)
        {
            if (string.IsNullOrWhiteSpace(periodo))
            {
                periodo = DateTime.Now.ToString("yyyy-MM");
            }

            // Validar que no exista ya para ese periodo
            var exists = await _context.Planillas.AnyAsync(p => p.Period == periodo);
            if (exists)
                return ApiResponse<PlanillaDto>.FailResponse($"Ya existe una planilla para el periodo {periodo}");

            // Crear planilla
            var planilla = new Sheet
            {
                Period = periodo,
                CreationDate = DateTime.Now,
                PaymentDate = DateTime.Now.AddDays(5), // Sugerencia
                State = "Pendiente"
            };

            _context.Planillas.Add(planilla);
            await _context.SaveChangesAsync(); // Save to get the ID

            // Obtener empleados activos
            var empleadosActivos = await _context.Empleados.Where(e => e.Active).ToListAsync();
            
            foreach (var emp in empleadosActivos)
            {
                var detalle = new SheetDetail
                {
                    PayrollId = planilla.Id,
                    EmployeeId = emp.Id,
                    SalaryBase = emp.SalaryBase,
                    ExtraHours = 0,
                    ExtraHoursAmount = 0,
                    Bonuses = 0,
                    Deductions = 0,
                    NetSalary = emp.SalaryBase, // Por defecto solo el salario base
                    Comments = "Generación automática"
                };
                _context.DetallesPlanilla.Add(detalle);
            }

            await _context.SaveChangesAsync();

            return ApiResponse<PlanillaDto>.SuccessResponse(MapToDto(planilla), $"Planilla para {periodo} generada automáticamente para {empleadosActivos.Count} empleados");
        }

        private static PlanillaDto MapToDto(Sheet p)
        {
            return new PlanillaDto
            {
                Id = p.Id,
                Period = p.Period,
                CreationDate = p.CreationDate,
                PaymentDate = p.PaymentDate,
                State = p.State
            };
        }
    }
}
