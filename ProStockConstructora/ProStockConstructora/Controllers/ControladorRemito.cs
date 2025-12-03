using BD;
using DTO.DTOs_Remitos;
using DTO.DTOs_Response;
using Microsoft.AspNetCore.Mvc;
using Repositorios.Implementaciones;
using Repositorios.Servicios;

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
        [HttpGet("obtener-numero-remito")]
        public async Task<ActionResult> ObtenerNumeroRemito()
        {
            var respuesta = await remitoServicio.ObtenerNumeroRemitoSiguiente();
            if (!respuesta.Estado)
                return StatusCode(500, respuesta);
            return Ok(respuesta);
        }

        [HttpGet("obtener-nota-de-pedido/{NotaDePedidoId:long}/deposito/{DepositoId:long}")]
        public async Task<ActionResult> ObtenerNotaDePedidoParaRemito(long NotaDePedidoId, long DepositoId)
        {
            var res = await remitoServicio.ObtenerNotaDePedidoParaRemito(NotaDePedidoId, DepositoId);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }
        [HttpPost]
        public async Task<ActionResult> CrearRemito([FromBody] CrearRemitoDTO remitoDTO)
        {
            var res = await remitoServicio.CrearRemito(remitoDTO);
            if (res.Estado) return StatusCode(201, res);
            else return StatusCode(500, res);
        }
    }
}
