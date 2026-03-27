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
    public class DetallePlanillaService : IDetallePlanillaService
    {
        private readonly PayrollDbContext _context;

        public DetallePlanillaService(PayrollDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DetallePlanillaDto>> GetByPlanillaAsync(int planillaId)
        {
            return await _context.DetallesPlanilla
                .Include(d => d.Empleado)
                .Where(d => d.PlanillaId == planillaId)
                .Select(d => MapToDto(d))
                .ToListAsync();
        }

        public async Task<IEnumerable<DetallePlanillaDto>> GetByEmpleadoAsync(int empleadoId)
        {
            return await _context.DetallesPlanilla
                .Include(d => d.Empleado)
                .Where(d => d.EmpleadoId == empleadoId)
                .Select(d => MapToDto(d))
                .ToListAsync();
        }

        public async Task<DetallePlanillaDto?> GetByIdAsync(int id)
        {
            var detalle = await _context.DetallesPlanilla
                .Include(d => d.Empleado)
                .FirstOrDefaultAsync(d => d.Id == id);
            return detalle == null ? null : MapToDto(detalle);
        }

        public async Task<ApiResponse<DetallePlanillaDto>> CreateAsync(DetallePlanillaCreateUpdateDto dto)
        {
            var empleado = await _context.Empleados.FindAsync(dto.EmpleadoId);
            if (empleado == null)
                return ApiResponse<DetallePlanillaDto>.FailResponse("Empleado no encontrado");

            var planilla = await _context.Planillas.FindAsync(dto.PlanillaId);
            if (planilla == null)
                return ApiResponse<DetallePlanillaDto>.FailResponse("Planilla no encontrada");

            if (planilla.Estado == "Pagada")
                return ApiResponse<DetallePlanillaDto>.FailResponse("No se puede agregar detalles a una planilla pagada");

            var detalle = new DetallePlanilla
            {
                PlanillaId = dto.PlanillaId,
                EmpleadoId = dto.EmpleadoId,
                SalarioBase = empleado.SalarioBase,
                HorasExtra = dto.HorasExtra,
                MontoHorasExtra = dto.MontoHorasExtra,
                Bonificaciones = dto.Bonificaciones,
                Deducciones = dto.Deducciones,
                Comentarios = dto.Comentarios
            };

            detalle.SalarioNeto = CalculateNeto(detalle);

            _context.DetallesPlanilla.Add(detalle);
            await _context.SaveChangesAsync();

            // Reload to get Navigation property for mapping
            await _context.Entry(detalle).Reference(d => d.Empleado).LoadAsync();

            return ApiResponse<DetallePlanillaDto>.SuccessResponse(MapToDto(detalle), "Detalle creado exitosamente");
        }

        public async Task<ApiResponse<DetallePlanillaDto>> UpdateAsync(int id, DetallePlanillaCreateUpdateDto dto)
        {
            var detalle = await _context.DetallesPlanilla.Include(d => d.Planilla).FirstOrDefaultAsync(d => d.Id == id);
            if (detalle == null)
                return ApiResponse<DetallePlanillaDto>.FailResponse("Detalle no encontrado");

            if (detalle.Planilla?.Estado == "Pagada")
                return ApiResponse<DetallePlanillaDto>.FailResponse("No se puede modificar detalles de una planilla pagada");

            detalle.HorasExtra = dto.HorasExtra;
            detalle.MontoHorasExtra = dto.MontoHorasExtra;
            detalle.Bonificaciones = dto.Bonificaciones;
            detalle.Deducciones = dto.Deducciones;
            detalle.Comentarios = dto.Comentarios;

            detalle.SalarioNeto = CalculateNeto(detalle);

            await _context.SaveChangesAsync();
            
            // Reload to get Navigation property for mapping
            await _context.Entry(detalle).Reference(d => d.Empleado).LoadAsync();
            
            return ApiResponse<DetallePlanillaDto>.SuccessResponse(MapToDto(detalle), "Detalle actualizado exitosamente");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var detalle = await _context.DetallesPlanilla.Include(d => d.Planilla).FirstOrDefaultAsync(d => d.Id == id);
            if (detalle == null)
                return ApiResponse<bool>.FailResponse("Detalle no encontrado");

            if (detalle.Planilla?.Estado == "Pagada")
                return ApiResponse<bool>.FailResponse("No se puede eliminar detalles de una planilla pagada");

            _context.DetallesPlanilla.Remove(detalle);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Detalle eliminado exitosamente");
        }

        private static decimal CalculateNeto(DetallePlanilla d)
        {
            // SalarioNeto = SalarioBase + MontoHorasExtra + Bonificaciones - Deducciones
            return d.SalarioBase + d.MontoHorasExtra + d.Bonificaciones - d.Deducciones;
        }

        private static DetallePlanillaDto MapToDto(DetallePlanilla d)
        {
            return new DetallePlanillaDto
            {
                Id = d.Id,
                PlanillaId = d.PlanillaId,
                EmpleadoId = d.EmpleadoId,
                NombreEmpleado = d.Empleado != null ? $"{d.Empleado.Nombre} {d.Empleado.Apellido}" : "N/A",
                SalarioBase = d.SalarioBase,
                HorasExtra = d.HorasExtra,
                MontoHorasExtra = d.MontoHorasExtra,
                Bonificaciones = d.Bonificaciones,
                Deducciones = d.Deducciones,
                SalarioNeto = d.SalarioNeto,
                Comentarios = d.Comentarios
            };
        }
    }
}
