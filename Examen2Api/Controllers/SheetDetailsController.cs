using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Examen2Api.Dtos;
using Examen2Api.Services;

namespace Examen2Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetallePlanillaController : ControllerBase
    {
        private readonly ISheetDetailService _service;

        public DetallePlanillaController(ISheetDetailService service)
        {
            _service = service;
        }

        [HttpGet("planilla/{planillaId}")]
        public async Task<ActionResult<IEnumerable<DetallePlanillaDto>>> GetByPlanilla(int planillaId)
        {
            var detalles = await _service.GetByPlanillaAsync(planillaId);
            return Ok(detalles);
        }

        [HttpGet("empleado/{empleadoId}")]
        public async Task<ActionResult<IEnumerable<DetallePlanillaDto>>> GetByEmpleado(int empleadoId)
        {
            var detalles = await _service.GetByEmpleadoAsync(empleadoId);
            return Ok(detalles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DetallePlanillaDto>> GetById(int id)
        {
            var detalle = await _service.GetByIdAsync(id);
            if (detalle == null)
                return NotFound(ApiResponse<DetallePlanillaDto>.FailResponse("Detalle no encontrado"));

            return Ok(detalle);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<DetallePlanillaDto>>> Create(DetallePlanillaCreateUpdateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<DetallePlanillaDto>>> Update(int id, DetallePlanillaCreateUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (!result.Success)
                return BadRequest(result);

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
    }
}