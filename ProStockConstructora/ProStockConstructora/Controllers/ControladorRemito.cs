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
        
        
    }
}
