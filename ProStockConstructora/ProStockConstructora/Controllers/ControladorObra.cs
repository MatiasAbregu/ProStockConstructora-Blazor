using BD;
using DTO.DTOs_Obras;
using DTO.DTOs_Response;
using DTO.DTOs_Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositorios.Implementaciones;
using Repositorios.Servicios;
using System.Diagnostics;

namespace ProStockConstructora.Controllers
{
    [Route("api/obra")]
    [ApiController]

    public class ControladorObra : ControllerBase
    {
        private readonly AppDbContext baseDeDatos;
        private readonly IObraServicio obraServicio;

        public ControladorObra(AppDbContext baseDeDatos, IObraServicio obraServicio)
        {
            this.baseDeDatos = baseDeDatos;
            this.obraServicio = obraServicio;
        }

        [HttpGet("empresa/{EmpresaId:long}")]
        public async Task<IActionResult> ObtenerObrasDeEmpresa(long EmpresaId)
        {
            var res = await obraServicio.ObtenerObrasDeEmpresa(EmpresaId);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> ObtenerObraPorId(long id)
        {
            var res = await obraServicio.ObtenerObraPorId(id);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }

        [HttpPost("obras-usuario")]
        public async Task<IActionResult> ObtenerObrasPorUsuario(DatosUsuario usuario)
        {
            var res = await obraServicio.ObtenerObrasPorUsuario(usuario);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }


        [HttpPost]
        public async Task<IActionResult> CrearObra([FromBody] CrearObraDTO obraDTO)
        {
            var res = await obraServicio.CrearObra(obraDTO);
            if (res.Estado)
                return StatusCode(200, res);
            return StatusCode(500, res);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> ActualizarObra(long id, [FromBody] ObraActualizarDTO obraDTO)
        {
            if (id != obraDTO.Id)
                return StatusCode(409, new Response<string>()
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error al intentar actualizar la obra.",
                    Objeto = null
                });

            var res = await obraServicio.ActualizarObra(id, obraDTO);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> FinalizarObra(long id)
        {
            var res = await obraServicio.FinalizarObra(id);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }
    }
}