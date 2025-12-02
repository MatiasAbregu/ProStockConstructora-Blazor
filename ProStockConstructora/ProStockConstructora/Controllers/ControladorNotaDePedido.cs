using BD;
using DTO.DTOs_NotaDePedido;
using Repositorios.Implementaciones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProStockConstructora.Client.Pages.Deposito;
using System.Data;
using DTO.DTOs_Usuarios;

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

        [HttpGet("obtener/{DepositoId:long}")]
        public async Task<ActionResult> ObtenerNotasDePedidoPorDepositoId(long DepositoId)
        {
            var respuesta = await notaDePedidoServicio.ObtenerNotasDePedidoPorDepositoId(DepositoId);
            if (!respuesta.Estado)
                return StatusCode(500, respuesta);
            return Ok(respuesta);
        }

        [HttpGet("obtener-pendiente/{DepositoId:long}")]
        public async Task<ActionResult> ObtenerNotasDePedidoPendientesPorDepositoId(long DepositoId)
        {
            var res = await notaDePedidoServicio.ObtenerNotasDePedidoPendientesPorDepositoId(DepositoId);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }

        [HttpPost("obtener-notas-pendientes")]
        public async Task<ActionResult> ObtenerNotasDePedidoPendientes(DatosUsuario usuario)
        {
            var res = await notaDePedidoServicio.ObtenerNotasDePedidoPendientes(usuario);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }

        [HttpGet("obtener-detalles-nota/{NotaDePedidoId:long}")]
        public async Task<ActionResult> ObtenerDetallesNotaDePedidoPorId(long NotaDePedidoId)
        {
            var respuesta = await notaDePedidoServicio.ObtenerDetallesNotaDePedidoPorId(NotaDePedidoId);
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

        [HttpPut("{NotaDePedidoId:long}")]
        public async Task<ActionResult> ModificarEstadosDeDetallesDeNotaDePedido(long NotaDePedidoId, [FromBody] List<VerDetalleNotadePedidoDTO> detalles)
        {
            var res = await notaDePedidoServicio.ActualizarEstadosNotaDePedido(NotaDePedidoId, detalles);
            if (res.Estado) return StatusCode(200, res);
            else return StatusCode(500, res);
        }
    }
}