using ProStockConstructora;
using BD.Modelos;
using DTO.DTOs_Depositos;
using Repositorios.Implementaciones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using DTO.DTOs_Response;
using DTO.DTOs_Usuarios;

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


        [HttpGet("{id:int}")]
        public async Task<ActionResult<VerDepositoDTO>> ObtenerDepositoPorId([FromRoute] int id)
        {
            Response<List<VerDepositoDTO>> res = await depositoServicio.ObtenerDepositoPorId(id);
            if (res.Estado) return Ok(res.Objeto);
            else return StatusCode(500, res.Mensaje);
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

        [HttpPut("actualizar/{id:int}")]
        public async Task<ActionResult<string>> ActualizarDeposito([FromRoute] int id, [FromBody] DepositoAsociarDTO e)
        {
            Response<string> res = await depositoServicio.ActualizarDeposito(id, e);
            if (res.Estado) return Ok(e);
            else return StatusCode(500, res);
        }
    }
}