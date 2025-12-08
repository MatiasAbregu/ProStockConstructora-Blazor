using BD.Modelos;
using DTO.DTOs_Depositos;
using DTO.DTOs_Response;
using DTO.DTOs_Usuarios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProStockConstructora;
using Repositorios.Implementaciones;
using Repositorios.Servicios;
using System.Collections.Generic;

namespace ProStockConstructora.Controllers
{
    [Route("api/deposito")]
    [ApiController]
    public class ControladorDeposito : ControllerBase
    {
        private readonly IDepositoServicio depositoServicio;

        public ControladorDeposito(IDepositoServicio depositoServicio)
        {
            this.depositoServicio = depositoServicio;
        }

        [HttpGet("{Id:long}/obtener-codigo-iso")]
        public async Task<IActionResult> ObtenerCodigoISODeDeposito([FromRoute] long Id)
        {
            var res = await depositoServicio.ObtenerCodigoISO(Id);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }

        [HttpGet("empresa/{EmpresaId:long}")]
        public async Task<IActionResult> ObtenerDepositosDeEmpresa(long EmpresaId)
        {
            var res = await depositoServicio.ObtenerDepositosDeEmpresa(EmpresaId);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<VerDepositoDTO>> ObtenerDepositoPorId([FromRoute] long id)
        {
            var res = await depositoServicio.ObtenerDepositoPorId(id);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }

        [HttpPost]
        public async Task<ActionResult> CrearDeposito([FromBody] DepositoAsociarDTO e)
        {
            var res = await depositoServicio.CrearDeposito(e);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }

        [HttpPost("depositos-usuario")]
        [HttpPost("depositos-usuario/{ObraId:long?}")]
        public async Task<ActionResult<Response<List<VerDepositoDTO>>>> ObtenerDepositosPorUsuario(DatosUsuario usuario, long? ObraId)
        {
            var res = await depositoServicio.ObtenerDepositosPorUsuario(usuario, ObraId);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<string>> ActualizarDeposito([FromRoute] long id, [FromBody] DepositoAsociarDTO e)
        {
            if (id != e.Id)
                return StatusCode(409, new Response<string>()
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error al intentar actualizar el depósito.",
                    Objeto = null
                });

            var res = await depositoServicio.ActualizarDeposito(id, e);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }
    }
}