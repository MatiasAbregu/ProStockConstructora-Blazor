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
<<<<<<< HEAD
using DTO.DTOs_Obras;
using DTO.DTOs_Response;
=======
>>>>>>> a8b31e8c0c543069e3149da4a07c437b47cf2a54

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

<<<<<<< HEAD
        [HttpGet("empresa/{EmpresaId:int}")] //obras de la empresa
        public async Task<IActionResult> ObtenerObras(int EmpresaId)
=======
        [HttpGet("empresa/{EmpresaId:long}")]
        public async Task<IActionResult> ObtenerObrasDeEmpresa(long EmpresaId)
>>>>>>> a8b31e8c0c543069e3149da4a07c437b47cf2a54
        {
            var res = await obraServicio.ObtenerObrasDeEmpresa(EmpresaId);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }

        //[HttpGet("{id:int}")] 
        //public async Task<IActionResult> ObtenerObraPorId(int obraId)
        //{
        //    Response<List<VerObraDTO>>
        //    resultado = await obraServicio.ObtenerObraPorId(obraId);
        //    if (!resultado.Estado)
        //        return StatusCode(500, "Error al obtener la obra.");
        //    else if (resultado.Objeto == null)
        //        return StatusCode(200, "No existe la obra con el ID proporcionado.");
        //    return Ok(resultado.Objeto);
        //}

<<<<<<< HEAD
        //[HttpGet("codigo/{codigoObra}")]
        //public async Task<IActionResult> ObtenerObraPorCodigoObra(string codigoObra)
        //{
        //    Response<List<VerObraDTO>>
        //    resultado = await obraServicio.ObtenerObrasPorCodigoObra(codigoObra);
        //    if (!resultado.Estado)
        //        return StatusCode(500, "Error al obtener la obra.");
        //    else if (resultado.Objeto == null || resultado.Objeto.Count == 0)
        //        return StatusCode(200, "No existe la obra con el código proporcionado.");
        //    return Ok(resultado.Objeto);
        //}
=======
        [HttpPost("obras-usuario")]
        public async Task<IActionResult> ObtenerObrasPorUsuario(DatosUsuario usuario)
        {
            var res = await obraServicio.ObtenerObrasPorUsuario(usuario);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }
>>>>>>> a8b31e8c0c543069e3149da4a07c437b47cf2a54

        [HttpPost] // habria que pasarle el id de la empresa
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
   


