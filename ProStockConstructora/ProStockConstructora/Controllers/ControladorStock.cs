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
        [HttpPost("obtener-stock")]

        public async Task<ActionResult> ObtenerStockPorDeposito(List<long> ObrasId)
        {
            var respuesta = await stockServicio.ObtenerStockPorObrasId(ObrasId);
            if (!respuesta.Estado)
                return StatusCode(500,respuesta);
            else
                return Ok(respuesta);
        }
    }
}
