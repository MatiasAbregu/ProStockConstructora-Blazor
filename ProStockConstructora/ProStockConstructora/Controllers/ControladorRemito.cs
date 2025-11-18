using BD;
using DTO.DTOs_Remitos;
using DTO.DTOs_Response;
using Microsoft.AspNetCore.Mvc;
using Repositorios.Implementaciones;

namespace ProStockConstructora.Controllers
{
    [Route("api/remito")]
    [ApiController]
    public class ControladorRemito : ControllerBase
    {
        private readonly IRemitoServicio remitoServicio;

        public ControladorRemito(IRemitoServicio remitoServicio)
        {
            this.remitoServicio = remitoServicio;
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<VerRemitoDTO>> ObtenerRemitoPorId(long id)
        {
            Response<List<VerRemitoDTO>> response = await remitoServicio.ObtenerRemitoPorId(id);
            if (response.Estado) return Ok(response.Objeto);
            else return StatusCode(500, response);
        }
        [HttpPost]
        public async Task<ActionResult>CrearRemito([FromBody] CrearRemitoDTO remitoDTO)
        {
            var res = await remitoServicio.CrearRemito(remitoDTO);
            if (res.Estado) return Ok(res.Objeto);
            else return StatusCode(500, res);
        }
        [HttpPut("actualizar/{id:long}")]
        public async Task<ActionResult<string>> ActualizarRemito([FromRoute]long id, [FromBody] ActualizarRemitoDTO remitoDTO)
        {
            var res = await remitoServicio.ActualizarRemito(id, remitoDTO);
            if (res.Estado) return Ok(res.Objeto);
            else return StatusCode(500, res);
        }
    }
}
