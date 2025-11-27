using BD;
using DTO.DTOs_NotaDePedido;
using Repositorios.Implementaciones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProStockConstructora.Client.Pages.Deposito;

namespace ProStockConstructora.Controllers
{
    [ApiController]
    [Route("api/notadepedido")]

    public class ControladorNotaDePedido : ControllerBase
    {
        private readonly AppDbContext baseDeDatos;
        private readonly INotaDePedidoServicio notaDePedidoServicio;

        public ControladorNotaDePedido(AppDbContext baseDeDatos, INotaDePedidoServicio notaDePedidoServicio)
        {
            this.baseDeDatos = baseDeDatos;
            this.notaDePedidoServicio = notaDePedidoServicio;
        }
        

        [HttpGet("obtener-numero-nota")]
        public async Task<ActionResult> ObtenerNumeroNotaPedido()
        {
            var respuesta = await notaDePedidoServicio.ObtenerNumeroNotadePedidoSiguiente();
            if (!respuesta.Estado)
                return StatusCode(500, respuesta);
            return Ok(respuesta);
        }

        [HttpPost]
        public async Task<ActionResult> CrearNotadePedido(CrearNotaDePedidoDTO CrearNota) 
        {
            var respuesta = await notaDePedidoServicio.CrearNotaDePedido(CrearNota);
            if (!respuesta.Estado)
                return StatusCode(500, respuesta);
            return Ok(respuesta);
        }
    }
}
