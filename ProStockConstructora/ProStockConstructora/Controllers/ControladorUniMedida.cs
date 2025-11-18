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
    [Route("api/unidad-medida")]
    [ApiController]
    public class ControladorUniMedida : ControllerBase
    {
        private readonly AppDbContext baseDeDatos;
        private readonly IUnidadMedidaServicio unidadMedidaServicio;

        public ControladorUniMedida(AppDbContext baseDeDatos, IUnidadMedidaServicio unidadMedidaServicio)
        {
            this.baseDeDatos = baseDeDatos;
            this.unidadMedidaServicio = unidadMedidaServicio;
        }

        [HttpPost]
        public async Task<IActionResult> CrearUnidadMedida([FromBody] UnidadDeMedidaDTO unidadDeMedidaDTO)
        {
            var res = await unidadMedidaServicio.UnidadDeMedidaCargar(unidadDeMedidaDTO);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerUnidadesDeMedida()
        {
            var resultado = await unidadMedidaServicio.ObtenerUnidadesDeMedida();
            if (!resultado.Estado)
                return StatusCode(500, resultado);
            return Ok(resultado);
        }

        [HttpPut("{Id:long}")]
        public async Task<IActionResult> ModificarUnidadMedida([FromBody] UnidadDeMedidaDTO unidadDeMedidaDTO,[FromRoute] long Id)
        {
            Response<string> resultado = await unidadMedidaServicio.UnidadDeMedidaModificar(unidadDeMedidaDTO, Id);
            if (!resultado.Estado)
                return StatusCode(500, resultado.Mensaje);
            return Ok(resultado.Mensaje);
        }
    }
}
