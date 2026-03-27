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
    public class EmpleadoService : IEmpleadoService
    {
        private readonly PayrollDbContext _context;

        public EmpleadoService(PayrollDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmpleadoDto>> GetAllAsync()
        {
            return await _context.Empleados
                .Select(e => MapToDto(e))
                .ToListAsync();
        }

        public async Task<IEnumerable<EmpleadoDto>> GetActivosAsync()
        {
            return await _context.Empleados
                .Where(e => e.Activo)
                .Select(e => MapToDto(e))
                .ToListAsync();
        }

        public async Task<EmpleadoDto?> GetByIdAsync(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            return empleado == null ? null : MapToDto(empleado);
        }

        public async Task<ApiResponse<EmpleadoDto>> CreateAsync(EmpleadoCreateUpdateDto dto)
        {
            var exists = await _context.Empleados.AnyAsync(e => e.Documento == dto.Documento);
            if (exists)
                return ApiResponse<EmpleadoDto>.FailResponse($"Ya existe un empleado con el documento {dto.Documento}");

            var empleado = new Empleado
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Documento = dto.Documento,
                FechaContratacion = dto.FechaContratacion,
                Departamento = dto.Departamento,
                PuestoTrabajo = dto.PuestoTrabajo,
                SalarioBase = dto.SalarioBase,
                Activo = true
            };

            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();

            return ApiResponse<EmpleadoDto>.SuccessResponse(MapToDto(empleado), "Empleado creado exitosamente");
        }

        public async Task<ApiResponse<EmpleadoDto>> UpdateAsync(int id, EmpleadoCreateUpdateDto dto)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
                return ApiResponse<EmpleadoDto>.FailResponse("Empleado no encontrado");

            var exists = await _context.Empleados.AnyAsync(e => e.Documento == dto.Documento && e.Id != id);
            if (exists)
                return ApiResponse<EmpleadoDto>.FailResponse($"Ya existe otro empleado con el documento {dto.Documento}");

            empleado.Nombre = dto.Nombre;
            empleado.Apellido = dto.Apellido;
            empleado.Documento = dto.Documento;
            empleado.FechaContratacion = dto.FechaContratacion;
            empleado.Departamento = dto.Departamento;
            empleado.PuestoTrabajo = dto.PuestoTrabajo;
            empleado.SalarioBase = dto.SalarioBase;

            await _context.SaveChangesAsync();
            return ApiResponse<EmpleadoDto>.SuccessResponse(MapToDto(empleado), "Empleado actualizado exitosamente");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
                return ApiResponse<bool>.FailResponse("Empleado no encontrado");

            // Baja lógica
            empleado.Activo = false;
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Empleado dado de baja exitosamente");
        }

        private static EmpleadoDto MapToDto(Empleado e)
        {
            return new EmpleadoDto
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Apellido = e.Apellido,
                Documento = e.Documento,
                FechaContratacion = e.FechaContratacion,
                Departamento = e.Departamento,
                PuestoTrabajo = e.PuestoTrabajo,
                SalarioBase = e.SalarioBase,
                Activo = e.Activo
            };
        }
    }
}
