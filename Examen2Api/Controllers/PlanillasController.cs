using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Examen2Api.Dtos;
using Examen2Api.Services;

namespace Examen2Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanillasController : ControllerBase
    {
        private readonly IPlanillaService _service;

        public PlanillasController(IPlanillaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlanillaDto>>> GetAll()
        {
            var planillas = await _service.GetAllAsync();
            return Ok(planillas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlanillaDto>> GetById(int id)
        {
            var planilla = await _service.GetByIdAsync(id);
            if (planilla == null)
                return NotFound(ApiResponse<PlanillaDto>.FailResponse("Planilla no encontrada"));

            return Ok(planilla);
        }

        [HttpGet("periodo/{periodo}")]
        public async Task<ActionResult<IEnumerable<PlanillaDto>>> GetByPeriodo(string periodo)
        {
            var planillas = await _service.GetByPeriodoAsync(periodo);
            return Ok(planillas);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PlanillaDto>>> Create(PlanillaCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<PlanillaDto>>> Update(int id, PlanillaCreateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPut("{id}/estado")]
        public async Task<ActionResult<ApiResponse<PlanillaDto>>> UpdateEstado(int id, [FromBody] PlanillaEstadoUpdateDto dto)
        {
            var result = await _service.UpdateEstadoAsync(id, dto.Estado);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("generar")]
        public async Task<ActionResult<ApiResponse<PlanillaDto>>> Generar(string periodo)
        {
            var result = await _service.GenerarPlanillaAutomatica(periodo);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
