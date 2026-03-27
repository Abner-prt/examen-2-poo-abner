using Microsoft.EntityFrameworkCore;
using Examen2Api.Data;
using Examen2Api.Dtos;
using Examen2Api.Entities;

namespace Examen2Api.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly PayrollDbContext _context;

        public EmployeeService(PayrollDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            return await _context.Empleados
                .Select(e => MapToDto(e))
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeDto>> GetActivosAsync()
        {
            return await _context.Empleados
                .Where(e => e.Active)
                .Select(e => MapToDto(e))
                .ToListAsync();
        }

        public async Task<EmployeeDto> GetByIdAsync(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            return empleado == null ? null : MapToDto(empleado);
        }

        public async Task<ApiResponse<EmployeeDto>> CreateAsync(EmployeeCreateUpdateDto dto)
        {
            var exists = await _context.Empleados.AnyAsync(e => e.Document == dto.Document);
            if (exists)
                return ApiResponse<EmployeeDto>.FailResponse($"Ya existe un empleado con el documento {dto.Document}");

            var empleado = new EmployeeEntity
            {
                Name = dto.Name,
                LastName = dto.LastName,
                Document = dto.Document,
                HireDate = dto.HireDate,
                Department = dto.Department,
                Position = dto.Position,
                SalaryBase = dto.SalaryBase,
                Active = true
            };

            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();

            return ApiResponse<EmployeeDto>.SuccessResponse(MapToDto(empleado), "Empleado creado exitosamente");
        }

        public async Task<ApiResponse<EmployeeDto>> UpdateAsync(int id, EmployeeCreateUpdateDto dto)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
                return ApiResponse<EmployeeDto>.FailResponse("Empleado no encontrado");

            var exists = await _context.Empleados.AnyAsync(e => e.Document == dto.Document && e.Id != id);
            if (exists)
                return ApiResponse<EmployeeDto>.FailResponse($"Ya existe otro empleado con el documento {dto.Document}");

            empleado.Name = dto.Name;
            empleado.LastName = dto.LastName;
            empleado.Document = dto.Document;
            empleado.HireDate = dto.HireDate;
            empleado.Department = dto.Department;
            empleado.Position = dto.Position;
            empleado.SalaryBase = dto.SalaryBase;

            await _context.SaveChangesAsync();
            return ApiResponse<EmployeeDto>.SuccessResponse(MapToDto(empleado), "Empleado actualizado exitosamente");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
                return ApiResponse<bool>.FailResponse("Empleado no encontrado");

            // Baja lógica
            empleado.Active = false;
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Empleado dado de baja exitosamente");
        }

        private static EmployeeDto MapToDto(EmployeeEntity e)
        {
            return new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                LastName = e.LastName,
                Document = e.Document,
                HireDate = e.HireDate,
                Department = e.Department,
                Position = e.Position,
                SalaryBase = e.SalaryBase,
                Active = e.Active
            };
        }
    }
}
