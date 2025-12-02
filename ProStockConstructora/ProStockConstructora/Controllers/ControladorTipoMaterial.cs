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
    [Route("api/tipo-material")]
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

        [HttpGet("empresa/{EmpresaId:long}")]
        public async Task<IActionResult> ObtenerTiposDeMaterial(long EmpresaId)
        {
            var resultado = await tipoMaterialServicio.ObtenerTiposDeMaterial(EmpresaId);
            if (resultado.Estado) return StatusCode(200, resultado);
            else return StatusCode(500, resultado);
        }

        [HttpGet("tipoMaterial/{Id:long}")]
        public async Task<IActionResult> ObtenerTipoMaterialPorId(long Id)
        {
            Response<TipoMaterialDTO> resultado = await tipoMaterialServicio.ObtenerTipoMaterialPorId(Id);
            if (!resultado.Estado)
                return StatusCode(500, resultado);
            return Ok(resultado);
        }

        [HttpPost("{empresaId:long}")]
        public async Task<IActionResult> CrearTipoMaterial(long empresaId, [FromBody] TipoMaterialDTO tipoMaterialDTO)
        {
            Response<string> resultado = await tipoMaterialServicio.TipoMaterialCargar(tipoMaterialDTO, empresaId);
            if (!resultado.Estado)
                return StatusCode(500, resultado);
            return Ok(resultado);
        }

        [HttpPut("{Id:long}")]
        public async Task<IActionResult> ModificarTipoMaterial([FromRoute] long Id, [FromBody] TipoMaterialDTO tipoMaterialDTO)
        {
            Response<string> resultado = await tipoMaterialServicio.TipoMaterialModificar(tipoMaterialDTO, Id);
            if (!resultado.Estado)
                return StatusCode(500, resultado);
            return Ok(resultado);
        }
    }
}
