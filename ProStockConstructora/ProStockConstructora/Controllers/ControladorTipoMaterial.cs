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
    [Route("api/tipoMaterial")]
    [ApiController]
    public class ControladorTipoMaterial : ControllerBase
    {
        private readonly AppDbContext baseDeDatos;
        private readonly ITipoMaterialServicio tipoMaterialServicio;

        public ControladorTipoMaterial(AppDbContext baseDeDatos, ITipoMaterialServicio tipoMaterialServicio)
        {
            this.baseDeDatos = baseDeDatos;
            this.tipoMaterialServicio = tipoMaterialServicio;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTiposDeMaterial()
        {
            Response<List<TipoMaterialDTO>> resultado = await tipoMaterialServicio.ObtenerTiposDeMaterial();
            if (!resultado.Estado)
                return StatusCode(500, resultado.Mensaje);
            return Ok(resultado.Objeto);
        }

        [HttpPost]
        public async Task<IActionResult> CrearTipoMaterial([FromBody] TipoMaterialDTO tipoMaterialDTO)
        {
            Response<string> resultado = await tipoMaterialServicio.TipoMaterialCargar(tipoMaterialDTO);
            if (!resultado.Estado)
                return StatusCode(500, resultado.Mensaje);
            return Ok(resultado.Mensaje);
        }

        [HttpPut("{Id:long}")]
        public async Task<IActionResult> ModificarTipoMaterial([FromBody] TipoMaterialDTO tipoMaterialDTO, [FromRoute] long Id)
        {
            Response<string> resultado = await tipoMaterialServicio.TipoMaterialModificar(tipoMaterialDTO, Id);
            if (!resultado.Estado)
                return StatusCode(500, resultado.Mensaje);
            return Ok(resultado.Mensaje);
        }
    }
}
