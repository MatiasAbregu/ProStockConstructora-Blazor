using ProStockConstructora;
using BD.Modelos;
using DTO.DTOs_Depositos;
using Repositorios.Implementaciones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        
        [HttpGet("{id:int}")]
        public async Task<ActionResult<VerDepositoDTO>> ObtenerDepositoPorId([FromRoute] int id)
        {
            ValueTuple<bool, VerDepositoDTO> res = await depositoServicio.ObtenerDepositoPorId(id);
            if (res.Item1 && res.Item2 != null) return StatusCode(200, res.Item2);
            else if (res.Item1 && res.Item2 == null) return StatusCode(404, "No existe un depósito con ese ID.");
            else return StatusCode(500, res.Item2);
        }

        [HttpGet("obra/{obraId:int}")]
        public async Task<ActionResult<List<VerDepositoDTO>>> ObtenerDepositosPorObraId([FromRoute] int obraId)
        {
            var res = await depositoServicio.ObtenerDepositosPorObraId(obraId);
            if (res.Item1 && res.Item2 != null) return StatusCode(200, res.Item2);
            else if (res.Item1) return StatusCode(200, "No existen depósitos aún.");
            else return StatusCode(500, "Ocurrió un error en el servidor.");

        }

        [HttpPost("crear")]
        public async Task<ActionResult<string>> CrearDeposito([FromBody] DepositoAsociarDTO e)
        {
            ValueTuple<bool, string> res = await depositoServicio.CrearDeposito(e);
            if (res.Item1) return StatusCode(201, res.Item2);
            else if (res.Item2.Contains("existe")) return StatusCode(409, res.Item2);
            else return StatusCode(500, res.Item2);
        }

        [HttpPut("actualizar/{id:int}")]
        public async Task<ActionResult<string>> ActualizarDeposito([FromRoute] int id, [FromBody] DepositoAsociarDTO e)
        {
            if (id != e.Id) return StatusCode(409, "No se pudo realizar la operación");
            ValueTuple<bool, string> res = await depositoServicio.ActualizarDeposito(e);
            if (res.Item1) return StatusCode(200, res.Item2);
            else if (res.Item2.Contains("ya")) return StatusCode(409, res.Item2);
            else return StatusCode(500, res.Item2);
        }

        //[HttpDelete("eliminar/{id:int}")]
        //public async Task<ActionResult<string>> EliminarDeposito([FromRoute] int id)
        //{
        //    ValueTuple<bool, string> res = await depositoServicio.EliminarDeposito(id);
        //    if (res.Item1) return StatusCode(200, res.Item2);
        //    else if (res.Item2.Contains("No existe")) return StatusCode(404, res.Item2);
        //    else return StatusCode(500, res.Item2);
        //}

    }
}
