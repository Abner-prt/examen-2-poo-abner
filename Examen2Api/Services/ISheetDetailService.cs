using Examen2Api.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Examen2Api.Services
{
    public interface ISheetDetailService
    {
        Task<IEnumerable<DetallePlanillaDto>> GetByPlanillaAsync(int planillaId);
        Task<IEnumerable<DetallePlanillaDto>> GetByEmpleadoAsync(int empleadoId);
        Task<DetallePlanillaDto> GetByIdAsync(int id);
        Task<ApiResponse<DetallePlanillaDto>> CreateAsync(DetallePlanillaCreateUpdateDto dto);
        Task<ApiResponse<DetallePlanillaDto>> UpdateAsync(int id, DetallePlanillaCreateUpdateDto dto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}
