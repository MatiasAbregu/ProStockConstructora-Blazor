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
        public async  Task<ActionResult> ObtenerRemitoPorId([FromRoute] long id)
        {
            var res = await remitoServicio.ObtenerRemitoPorId(id);
            if (res.Estado) return Ok(res.Objeto);
            else return StatusCode(500, res);
        }
        [HttpGet("notadepedidos/{notaDePedidoId:long}")]
        public async Task<ActionResult> ObtenerRemitoPorNotaDePedidoId([FromRoute] long notaDePedidoId)
        {
            var res = await remitoServicio.ObtenerRemitoPorNotaDePedidoId(notaDePedidoId);
            if (res.Estado) return Ok(res.Objeto);
            else return StatusCode(500, res);
        }
        [HttpPost]
        public async Task<ActionResult>CrearRemito([FromBody] CrearRemitoDTO remitoDTO)
        {
            var res = await remitoServicio.CrearRemito(remitoDTO);
            if (res.Estado) return Ok(res.Objeto);
            else return StatusCode(500, res);
        }
        [HttpPut("{id:long}")]
        public async Task<ActionResult<string>> ActualizarRemito([FromRoute]long id, [FromBody] ActualizarRemitoDTO remitoDTO)
        {
            if (id != remitoDTO.Id)
                return StatusCode(409, new Response<string>()
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error al intentar actualizar el remito.",
                    Objeto = null
                });
            var res = await remitoServicio.ActualizarRemito(id, remitoDTO);
            if (res.Estado) return Ok(res.Objeto);
            else return StatusCode(500, res);
        }
    }
}
