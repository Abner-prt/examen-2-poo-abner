using Examen2Api.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Examen2Api.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllAsync();
        Task<IEnumerable<EmployeeDto>> GetActivosAsync();
        Task<EmployeeDto> GetByIdAsync(int id);
        Task<ApiResponse<EmployeeDto>> CreateAsync(EmployeeCreateUpdateDto dto);
        Task<ApiResponse<EmployeeDto>> UpdateAsync(int id, EmployeeCreateUpdateDto dto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}
