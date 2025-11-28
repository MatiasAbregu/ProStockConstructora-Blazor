using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositorios.Implementaciones;
using Repositorios.Servicios;

namespace ProStockConstructora.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class ControladorStock : ControllerBase
    {
        private readonly IStockServicio stockServicio;
        public ControladorStock(IStockServicio stockServicio)
        {
            this.stockServicio = stockServicio;
        }

        [HttpGet("obtener-stock/{UsuarioId:long}/{DepositoOrigenId:long}")]
        public async Task<ActionResult> ObtenerStockPorUsuarioId(long UsuarioId, long DepositoOrigenId)
        {
            var res = await stockServicio.ObtenerStocksDeEmpresaPorIdAdministrador(UsuarioId, DepositoOrigenId);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }

        [HttpPost("obtener-stock/{DepositoOrigenId:long}")]
        public async Task<ActionResult> ObtenerStockPorDeposito(List<long> ObrasId, long DepositoOrigenId)
        {
            var respuesta = await stockServicio.ObtenerStockPorObrasId(ObrasId, DepositoOrigenId);
            if (!respuesta.Estado)
                return StatusCode(500,respuesta);
            else
                return Ok(respuesta);
        }
        
    }
}
