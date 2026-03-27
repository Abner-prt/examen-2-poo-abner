using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Examen2Api.Dtos;
using Examen2Api.Services;

namespace Examen2Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadosController : ControllerBase
    {
        private readonly IEmpleadoService _service;

        public EmpleadosController(IEmpleadoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpleadoDto>>> GetAll()
        {
            var empleados = await _service.GetAllAsync();
            return Ok(empleados);
        }

        [HttpGet("activos")]
        public async Task<ActionResult<IEnumerable<EmpleadoDto>>> GetActivos()
        {
            var empleados = await _service.GetActivosAsync();
            return Ok(empleados);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmpleadoDto>> GetById(int id)
        {
            var empleado = await _service.GetByIdAsync(id);
            if (empleado == null)
                return NotFound(ApiResponse<EmpleadoDto>.FailResponse("Empleado no encontrado"));

            return Ok(empleado);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<EmpleadoDto>>> Create(EmpleadoCreateUpdateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<EmpleadoDto>>> Update(int id, EmpleadoCreateUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}
