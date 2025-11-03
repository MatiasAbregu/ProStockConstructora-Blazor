using BD;
using BD.Modelos;
using Repositorios.Implementaciones;
using Repositorios.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using BD.Enums;
using DTO.DTOs_MaterialesYmaquinarias;
using DTO.DTOs_Recursos;

namespace ProStockConstructora.Controllers
{
    [Route("api/recursos")]
    [ApiController]

    public class ControladorRecursos : ControllerBase
    {
        private readonly AppDbContext baseDeDatos;
        private readonly IRecursosServicio recursosServicio;

        public ControladorRecursos(AppDbContext baseDeDatos, IRecursosServicio recursosServicio)
        {
            this.baseDeDatos = baseDeDatos;
            this.recursosServicio = recursosServicio;
        }

        //[HttpGet("Obtenerrecursos/{EmpresaId}")]
        //public async Task<IActionResult> ObtenerRecursos(int EmpresaId)
        //{
        //   ValueTuple<bool, List<RecursosVerDTO>>
        //   resultado = await recursosServicio.RecursosVerDTO();
        //    if (!resultado.Item1)
        //        return StatusCode(500, "Error al obtener los materiales y maquinarias.");
        //    else if (resultado.Item2 == null || resultado.Item2.Count == 0)
        //        return StatusCode(200, "No hay materiales y maquinarias registradas en la empresa.");
        //    return Ok(resultado.Item2);
        //}

        [HttpGet("materialesYmaquinarias/{EmpresaId}")]
        public async Task<IActionResult> ObtenerTotalRecursos(int EmpresaId)
        {
            ValueTuple<bool, List<RecursosPagPrincipalDTO>>
            resultado = await recursosServicio.RecursosVerDTO(EmpresaId);
            if (!resultado.Item1)
                return StatusCode(200, "No hay recursos disponibles");
            else if (resultado.Item2 == null || resultado.Item2.Count == 0)
                return StatusCode(200, "No hay materiales y maquinarias registradas.");
            return Ok(resultado.Item2);
        }

        [HttpGet("deposito/{DepositoId}")]
        public async Task<IActionResult> ObtenerRecursosPorDeposito(int DepositoId)
        {
            ValueTuple<bool, List<RecursosVerDepositoDTO>>
            resultado = await recursosServicio.RecursosVerDepositoDTO(DepositoId);
            if (!resultado.Item1)
                return StatusCode(500, "Error al obtener los materiales y maquinarias.");
            else if (resultado.Item2 == null || resultado.Item2.Count == 0)
                return StatusCode(200, "No hay materiales y maquinarias registradas en el depósito.");
            return Ok(resultado.Item2);
        }

        [HttpGet("{stockId:int}")]
        public async Task<IActionResult> ObtenerRecursoPorStockId(int stockId)
        {
            var res = await recursosServicio.ObtenerRecursoPorStockId(stockId);

            if (res.Item1) 
                return StatusCode(200, res.Item2);
            else 
                return StatusCode(404, "No se encontro el stock.");
        }

        [HttpGet("verificar/{CodigoISO}")]
        public async Task<IActionResult> VerificarRecursoPorCodigoISO(string CodigoISO)
        {
            var res = await recursosServicio.VerificarRecursoPorCodigoISO(CodigoISO);
            if (res.Item1)
                return StatusCode(200, res.Item2);
            else 
                return StatusCode(404, res.Item2);
        }


        [HttpPost("{DepositoId:int}")]
        public async Task<IActionResult> RecursoCargar([FromBody] RecursosCargarDTO recursoCargarDTO, int DepositoId)
        {
            if (recursoCargarDTO == null)
                return BadRequest("El recurso no puede ser nulo.");
            var exito = await recursosServicio.RecursoCargar(recursoCargarDTO, DepositoId);
            if (!exito.Item1)
                return StatusCode(500, exito.Item2);
            return Ok("Recurso cargado con exito.");
        }

        [HttpPut("deposito/movimiento")]
        public async Task<IActionResult> RecursosTransladarAdeposito([FromBody] RecursosTransladarDepositoDTO recursosTransladarAdepositoDTO)
        {
            if (recursosTransladarAdepositoDTO == null)
                return BadRequest("El recurso no puede ser nulo.");
            var exito = await recursosServicio.RecursosTransladarAdeposito(recursosTransladarAdepositoDTO);
            if (!exito.Item1)
                return StatusCode(500, "Error al trasladar el recurso al deposito.");
            return Ok($"Recurso trasladado al deposito {recursosTransladarAdepositoDTO.DepositoDestinoId} con exito.");
        }

        //[HttpPut("deposito/actualizarstock/{DepositoId:int}")]
        //public async Task<IActionResult> RecursosActualizarStock([FromBody] RecursosActualizarDTO recursoActualizarDTO, int DepositoId)
        //{
        //    if (recursoActualizarDTO == null)
        //        return BadRequest("El recurso no puede ser nulo.");
        //    var exito = await recursosServicio.RecursosActualizarStock(recursoActualizarDTO, DepositoId);
        //    if (!exito.Item1)
        //        return StatusCode(500, exito.Item2);
        //    return Ok("Stock actualizado con exito.");
        //}

        [HttpDelete("deposito/eliminartock/{stockId:int}")]
        public async Task<IActionResult> RecursoEliminarStock(int stockId)
        {
            if (stockId <= 0)
                return StatusCode(404, "El ID del stock no puede ser menor o igual a cero.");
            if (stockId == 0)
                return StatusCode(404, "El ID del stock no existe");
            var exito = await recursosServicio.RecursoEliminarStock(stockId);
            if (!exito.Item1)
                return StatusCode(500, exito.Item2);
            return Ok("Stock eliminado con exito.");
        }
    }
}