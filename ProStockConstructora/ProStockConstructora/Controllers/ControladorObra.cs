using BD;
using DTO.DTOs_Obras;
using DTO.DTOs_Response;
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
            Response<List<ObraEmpresaDTO>>
            resultado = await obraServicio.ObtenerObrasDeEmpresa(EmpresaId);
            if (!resultado.Estado)
                return StatusCode(500, "Error al obtener las obras.");
            else return Ok(resultado);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerObraPorId(int id)
        {
            ValueTuple<bool, VerObraDTO>
            resultado = await obraServicio.ObtenerObraPorId(id);
            if (!resultado.Item1)
                return StatusCode(500, "Error al obtener la obra.");
            else if (resultado.Item2 == null)
                return StatusCode(200, "No existe la obra con el ID proporcionado.");
            return Ok(resultado.Item2);
        }


        [HttpPost]
        public async Task<IActionResult> CrearObra([FromBody] CrearObraDTO obraDTO)
        {
           ValueTuple<bool, string> resultado = await obraServicio.CrearObra(obraDTO);
            if (!resultado.Item1)
                return StatusCode(500, resultado.Item2);
            return Ok("Obra creada exitosamente.");
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
   


