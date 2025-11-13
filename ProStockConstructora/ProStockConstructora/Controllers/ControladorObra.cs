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

        [HttpPut("{id:int}")]
        public async Task<IActionResult> ActualizarObra(int id, [FromBody] ObraActualizarDTO obraDTO)
        {
            if (id != obraDTO.Id)
                return BadRequest("El ID de la obra no coincide.");
            ValueTuple<bool, string> resultado = await obraServicio.ActualizarObra(id, obraDTO);
            if (!resultado.Item1)
                return StatusCode(409, resultado.Item2);
            return Ok("Obra actualizada exitosamente.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> EliminarObra(int id)
        {
            var obra = await baseDeDatos.Obras.FindAsync(id);
            if (obra == null)
                return NotFound("No se encontró la obra con el ID proporcionado.");
            baseDeDatos.Obras.Remove(obra);
            await baseDeDatos.SaveChangesAsync();
            return Ok("Obra eliminada exitosamente.");
        }
    }
}