using Microsoft.AspNetCore.Mvc;
using Repositorios.Implementaciones;

namespace ProStockConstructora.Controllers
{
    [Route("api/movimientos")]
    [ApiController]
    public class ControladorMovimientos : ControllerBase
    {
        private readonly IMovimientoServicio movimientoServicio;

        public ControladorMovimientos(IMovimientoServicio movimientoServicio)
        {
            this.movimientoServicio = movimientoServicio;
        }

        [HttpGet("{EmpresaId:long}")]
        public async Task<IActionResult> ObtenerMovimientosPorEmpresa(long EmpresaId)
        {
            var res = await movimientoServicio.ObtenerMovimientosPorEmpresa(EmpresaId);
            if(res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }
    }
}