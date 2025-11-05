using BD;
using BD.Modelos;
using DTO.DTOs_Recursos;
using DTO.DTOs_Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositorios.Implementaciones;
using Repositorios.Servicios;

namespace ProStockConstructora.Controllers
{
    [Route("api/uniMedida")]
    [ApiController]
    public class ControladorUniMedida : ControllerBase
    {
        private readonly AppDbContext baseDeDatos;
        private readonly IUnidadMedidaServicio unidadMedidaServicio;

        public ControladorUniMedida(AppDbContext baseDeDatos)
        {
            this.baseDeDatos = baseDeDatos;
        }

        [HttpPost("{UnidadMedida:int}")]
        public async Task<IActionResult> CrearUnidadMedida([FromBody] UnidadDeMedidaDTO unidadDeMedidaDTO, int UnidadMedida)
        {
            Response<string> resultado = await unidadMedidaServicio.UnidadDeMedidaCargar(unidadDeMedidaDTO);
            if (!resultado.Estado)
                return StatusCode(500, resultado.Mensaje);
            return Ok(resultado.Mensaje);
        }

        [HttpGet("{UnidadMedida:int}")]
        public async Task<IActionResult> ObtenerUnidadesDeMedida(int UnidadMedida)
        {
            Response<List<UnidadDeMedidaDTO>> resultado = await unidadMedidaServicio.ObtenerUnidadesDeMedida(UnidadMedida);
            if (!resultado.Estado)
                return StatusCode(500, resultado.Mensaje);
            return Ok(resultado.Objeto);
        }
    }
}
