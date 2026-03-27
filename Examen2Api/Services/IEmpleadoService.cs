using Examen2Api.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Examen2Api.Services
{
    public interface IEmpleadoService
    {
        Task<IEnumerable<EmpleadoDto>> GetAllAsync();
        Task<IEnumerable<EmpleadoDto>> GetActivosAsync();
        Task<EmpleadoDto?> GetByIdAsync(int id);
        Task<ApiResponse<EmpleadoDto>> CreateAsync(EmpleadoCreateUpdateDto dto);
        Task<ApiResponse<EmpleadoDto>> UpdateAsync(int id, EmpleadoCreateUpdateDto dto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}
