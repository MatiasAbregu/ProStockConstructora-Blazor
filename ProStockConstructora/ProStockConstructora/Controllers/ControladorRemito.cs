using BD;
using DTO.DTOs_Remitos;
using DTO.DTOs_Response;
using DTO.DTOs_Usuarios;
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

        [HttpGet("{NotaDePedidoId:long}/{DepositoId:long}")]
        public async Task<ActionResult> ObtenerRemitosPorNotaDePedidoId(long NotaDePedidoId, long DepositoId)
        {
            var res = await remitoServicio.ObtenerRemitosPorNotaDePedido(NotaDePedidoId, DepositoId);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);  
        }
        
        [HttpGet("obtener-detalles-remito/{RemitoId:long}")]
        public async Task<ActionResult> ObtenerDetallesRemitoPorId(long RemitoId)
        {
            var res = await remitoServicio.ObtenerDetallesRemitoPorId(RemitoId);
            if (res.Estado) return StatusCode(200, res);
            return StatusCode(500, res);
        }
        
        [HttpGet("obtener-pendiente/{NotaDePedidoId:long}")]
        public async Task<ActionResult> ObtenerRemitosPendientesPorNotaDepedido(long NotaDePedidoId)
        {
            var res = await remitoServicio.ObtenerRemitosPendietesPorNotaDePedido(NotaDePedidoId);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }  

        [HttpPost("obtener-remitos-pendientes")]
        public async Task<ActionResult> ObtenerRemitosPendientes(DatosUsuario usuario)
        {
            var res = await remitoServicio.ObtenerRemitosPendientes(usuario);
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
        
        [HttpPut("{RemitoId:long}")]
        public async Task<ActionResult> ModificarEstadosDeDetallesDeRemito(long RemitoId, [FromBody] List<VerDetalleRemitoDTO> detalles)
        {
            var res = await remitoServicio.ActualizarEstadosRemito(RemitoId, detalles);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }

        [HttpDelete("{RemitoId:long}/{UsuarioId:long}")]
        public async Task<ActionResult> AnularRemito(long RemitoId, long UsuarioId)
        {
            var res = await remitoServicio.AnularRemito(RemitoId, UsuarioId);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }
    }
}
