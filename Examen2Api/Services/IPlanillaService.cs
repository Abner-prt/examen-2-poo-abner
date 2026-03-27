using Examen2Api.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Examen2Api.Services
{
    public interface IPlanillaService
    {
        Task<IEnumerable<PlanillaDto>> GetAllAsync();
        Task<PlanillaDto?> GetByIdAsync(int id);
        Task<IEnumerable<PlanillaDto>> GetByPeriodoAsync(string periodo);
        Task<ApiResponse<PlanillaDto>> CreateAsync(PlanillaCreateDto dto);
        Task<ApiResponse<PlanillaDto>> UpdateAsync(int id, PlanillaCreateDto dto);
        Task<ApiResponse<PlanillaDto>> UpdateEstadoAsync(int id, string estado);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<PlanillaDto>> GenerarPlanillaAutomatica(string periodo);
    }
}
