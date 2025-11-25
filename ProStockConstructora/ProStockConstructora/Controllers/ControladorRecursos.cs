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

        [HttpGet("empresa/{EmpresaId:long}")]
        public async Task<IActionResult> ObtenerRecursosDeEmpresa(long EmpresaId)
        {
            var res = await recursosServicio.ObtenerRecursosEmpresa(EmpresaId);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }

        [HttpGet("deposito/{DepositoId:long}")]
        public async Task<IActionResult> ObtenerRecursosDeDeposito(long DepositoId)
        {
            var res = await recursosServicio.ObtenerRecursosDeposito(DepositoId);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }

        [HttpGet("{RecursoId:long}")]
        [HttpGet("deposito/{DepositoId:long}/recurso/{RecursoId:long}")]
        public async Task<IActionResult> ObtenerRecursoDeDeposito(long? DepositoId, long RecursoId)
        {
            var res = await recursosServicio.ObtenerRecursoPorIdYODeposito(DepositoId, RecursoId);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }

        [HttpPost]
        public async Task<IActionResult> RecursoCrear([FromBody] RecursosCrearDTO recursoDTO)
        {
            var res = await recursosServicio.RecursoCrear(recursoDTO);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }

        [HttpPost("codigo-iso")]
        public async Task<IActionResult> RecursoAnadirPorCodigoISO(RecursoPorISODTO recursoDTO)
        {
            var res = await recursosServicio.RecursoAnadirPorISO(recursoDTO);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }

        [HttpPut("{RecursoId:long}")]
        [HttpPut("deposito/{DepositoId:long}/recurso/{RecursoId:long}")]
        public async Task<IActionResult> RecursosActualizar(long? DepositoId, long RecursoId, RecursosActualizarDTO recurso)
        {
            var res = await recursosServicio.RecursoActualizar(DepositoId, RecursoId, recurso);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }

    }
}