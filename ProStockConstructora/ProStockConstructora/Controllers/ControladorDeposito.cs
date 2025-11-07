using ProStockConstructora;
using BD.Modelos;
using DTO.DTOs_Depositos;
using Repositorios.Implementaciones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using DTO.DTOs_Response;

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

        [HttpGet]
        public async Task<ActionResult<List<VerDepositoDTO>>> ObtenerDepositos()
        {
            Response<List<VerDepositoDTO>> res = await depositoServicio.ObtenerDepositos();
            if (res.Estado) return Ok(res.Objeto);
            else return StatusCode(500, res.Mensaje);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<VerDepositoDTO>> ObtenerDepositoPorId([FromRoute] int id)
        {
            Response<List<VerDepositoDTO>> res = await depositoServicio.ObtenerDepositoPorId(id);
            if (res.Estado) return Ok(res.Objeto);
            else return StatusCode(500, res.Mensaje);
        }
       

        [HttpGet("obra/{obraId:int}")]
        public async Task<ActionResult<List<VerDepositoDTO>>> ObtenerDepositosPorObraId([FromRoute] int obraId)
        {
           Response<List<VerDepositoDTO>> res = await depositoServicio.ObtenerDepositosPorObraId(obraId);
            if (res.Estado) return Ok(res.Objeto);
            else return StatusCode(500, res.Mensaje);

        }

        [HttpPost("crear")]
        public async Task<ActionResult<int>> CrearDeposito([FromBody] DepositoAsociarDTO e)
        {
            Response<int> res = await depositoServicio.CrearDeposito(e);
            return res.Estado ? StatusCode(201, res.Objeto) : StatusCode(500, res.Mensaje);
        }
        
        [HttpPut("actualizar/{id:int}")]
        public async Task<ActionResult<string>> ActualizarDeposito([FromRoute] int id, [FromBody] DepositoAsociarDTO e)
        {
            Response<string> res = await depositoServicio.ActualizarDeposito(id, e);
            if (res.Estado) return Ok(res.Objeto);
            else return StatusCode(500, res.Mensaje);
        }

        [HttpDelete("eliminar/{id:long}")]
        public async Task<ActionResult<string>> EliminarDeposito(long id)
        {
            Response<string> res = await depositoServicio.EliminarDeposito(id);
            if (res.Estado) return Ok(res.Objeto);
            else return StatusCode(500, res.Mensaje);
        }

    }
}
