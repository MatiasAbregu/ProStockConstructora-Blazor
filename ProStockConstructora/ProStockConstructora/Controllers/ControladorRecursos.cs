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
using DTO.DTOs_Response;

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

        [HttpGet("materialesYmaquinarias/{EmpresaId}")]
        public async Task<IActionResult> ObtenerTotalRecursos(long EmpresaId)
        {
            Response<List<RecursosPagPrincipalDTO>>
            resultado = await recursosServicio.RecursosVerDTO(EmpresaId);
            if (!resultado.Estado)
                return StatusCode(500, resultado);
            return Ok(resultado);
        }

        [HttpGet("deposito/{DepositoId}")]
        public async Task<IActionResult> ObtenerRecursosPorDeposito(long DepositoId)
        {
            Response<List<RecursosVerDepositoDTO>>
            resultado = await recursosServicio.RecursosVerDepositoDTO(DepositoId);
            if (!resultado.Estado)
                return StatusCode(500, "Error al obtener los materiales y maquinarias.");
            else if (resultado.Objeto == null || resultado.Objeto.Count == 0)
                return StatusCode(200, "No hay materiales y maquinarias registradas en el depósito.");
            return Ok(resultado);
        }

        [HttpGet("{stockId:long}")]
        public async Task<IActionResult> ObtenerRecursoPorStockId(long stockId)
        {
            Response<List<RecursoStockVerDTO>>
            resultado = await recursosServicio.ObtenerRecursoPorStockId(stockId);
            if (!resultado.Estado)
                return StatusCode(500, "Error al obtener el recurso por stockId.");
            else if (resultado.Objeto == null || resultado.Objeto.Count == 0)
                return StatusCode(200, "No hay recursos registrados con ese stockId.");
            return Ok(resultado);
        }

        [HttpGet("verificar/{CodigoISO}")]
        public async Task<IActionResult> VerificarRecursoPorCodigoISO(string CodigoISO)
        {
            Response<object>
            resultado = await recursosServicio.VerificarRecursoPorCodigoISO(CodigoISO);
            if (!resultado.Estado)
                return StatusCode(500, "Error al verificar el recurso por CodigoISO.");
            return Ok(resultado);
        }

        [HttpPost("{empresaId:long}")]
        public async Task<IActionResult> RecursoCrear(long empresaId, [FromBody] RecursosCargarEmpresaDTO recursoCrearDTO)
        {
            Response<string> resultado = await recursosServicio.RecursoCargar(recursoCrearDTO, empresaId);
            if (!resultado.Estado)
                return StatusCode(500, resultado.Mensaje);
            return Ok(resultado.Mensaje);
        }

        [HttpPost("{depositoId:long}")]
        public async Task<IActionResult> RecursoCargar(long depositoId, [FromBody] RecursosCargarAdepositoDTO recursoCargarDTO)
        {
            Response<string> resultado = await recursosServicio.RecursoCargar(recursoCargarDTO, depositoId);
            if (!resultado.Estado)
                return StatusCode(500, resultado.Mensaje);
            return Ok(resultado.Mensaje);
        }

        [HttpPut("deposito/movimiento")]
        public async Task<IActionResult> RecursosTransladarAdeposito([FromBody] RecursosTransladarDepositoDTO recursosTransladarAdepositoDTO)
        {
            Response<string> resultado = await recursosServicio.RecursosTransladarAdeposito(recursosTransladarAdepositoDTO);
            if (!resultado.Estado)
                return StatusCode(500, resultado.Mensaje);
            return Ok(resultado.Mensaje);
        }

        [HttpPut("recurso/actualizar")]
        public async Task<IActionResult> RecursosActualizar([FromBody] RecursosActualizarDTO recursoActualizarDTO, long recursoId)
        {
           Response<string> resultado = await recursosServicio.RecursosActualizar(recursoActualizarDTO, recursoId);
            if (!resultado.Estado)
                return StatusCode(500, resultado.Mensaje);
            return Ok(resultado.Mensaje);
        }

        [HttpDelete("deposito/eliminarStock/{stockId:long}")]
        public async Task<IActionResult> RecursoEliminarStock(long stockId)
        {
            Response<string> resultado = await recursosServicio.RecursoEliminarStock(stockId);
            if (!resultado.Estado)
                return StatusCode(500, resultado.Mensaje);
            return Ok(resultado.Mensaje);
        }
    }
}