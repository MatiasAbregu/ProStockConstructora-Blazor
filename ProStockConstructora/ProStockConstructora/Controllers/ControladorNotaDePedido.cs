using BD;
using DTO.DTOs_NotaDePedido;
using Repositorios.Implementaciones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [HttpGet("obtener-notaspedidos/Numero/{DepositoId}")]
        public async Task<ActionResult<VerNotaDePedidoDTO>> ObtenerNotasDePedido(int DepositoId)
        {
           var respuesta = await notaDePedidoServicio.ObtenerNotasDePedido(DepositoId);
            if (respuesta == null)
                return StatusCode(500, "Error al obtener las notas de pedido.");
            return Ok(respuesta);
        }

        [HttpGet("obtener-notapedido/{NumeroNotaPedido}")]
        public async Task<ActionResult> ObtenerNotaDePedidoPorId(string NumeroNotaPedido)
        {
            var respuesta = await notaDePedidoServicio.ObtenerNotaDePedidoPorId(NumeroNotaPedido);
            if (respuesta == null)
                return StatusCode(500, "Error al obtener la nota de pedido.");  
              return Ok(respuesta);

        }

        [HttpGet("obtener-numero-nota")]
        public async Task<ActionResult> ObtenerNumeroNotaPedido()
        {
            var respuesta = await notaDePedidoServicio.ObtenerNumeroNotadePedidoSiguiente();
            if (!respuesta.Estado)
                return StatusCode(500, "Error al obtener el número de nota de pedido.");
            return Ok(respuesta);
        }

        // [HttpPost("crear-notapedido")]
        //public async Task<ActionResult> CrearNotaDePedido([FromBody] CrearNotaDePedidoDTO crearNotaDePedidoDTO)
        //{
        //    if (crearNotaDePedidoDTO == null)
        //        return BadRequest("El cuerpo de la solicitud no puede ser nulo.");
        //    var exito = await notaDePedidoServicio.CrearNotaDePedido(crearNotaDePedidoDTO);
        //    if (!exito.Item1)
        //        return StatusCode(500, exito.Item2);
        //    return Ok("Nota de pedido creada exitosamente.");

        //}





        //[HttpPut("actualizar-notapedido/{id:int}")]
        //public async Task<ActionResult> ActualizarNotaDePedido([FromRoute] int id, [FromBody] BD.Modelos.NotaDePedido notaDePedidoActualizada)
        //{
        //    try
        //    {
        //        var notaDePedidoExistente = await baseDeDatos.NotaDePedidos.FirstOrDefaultAsync(np => np.Id == id);
        //        if (notaDePedidoExistente == null)
        //        {
        //            return StatusCode(404, "No existe una nota de pedido con ese ID.");
        //        }
        //        // Actualizar los campos necesarios
        //        notaDePedidoExistente.NumeroNotaPedido = notaDePedidoActualizada.NumeroNotaPedido;
        //        notaDePedidoExistente.Material = notaDePedidoActualizada.Material;
        //        notaDePedidoExistente.Cantidad = notaDePedidoActualizada.Cantidad;
        //        notaDePedidoExistente.DepositoDestinoId = notaDePedidoActualizada.DepositoDestinoId;
        //        notaDePedidoExistente.FechaEmision = notaDePedidoActualizada.FechaEmision;
        //        notaDePedidoExistente.Estado = notaDePedidoActualizada.Estado;
        //        await baseDeDatos.SaveChangesAsync();
        //        return Ok(notaDePedidoExistente);
        //    } 
        //    catch (Exception)
        //    {
        //        return StatusCode(500, "Error al actualizar la nota de pedido.");
        //    }
        //}




        [HttpDelete("eliminar-notapedido/{id:int}")]
        public async Task<ActionResult> EliminarNotaDePedido([FromRoute] int id)
        {
            try
            {
                var notaDePedidoExistente = await baseDeDatos.NotaDePedidos.FirstOrDefaultAsync(np => np.Id == id);
                if (notaDePedidoExistente == null)
                {
                    return StatusCode(404, "No existe una nota de pedido con ese ID.");
                }
                baseDeDatos.NotaDePedidos.Remove(notaDePedidoExistente);
                await baseDeDatos.SaveChangesAsync();
                return Ok("Nota de pedido eliminada exitosamente.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al eliminar la nota de pedido.");
            }
        }
    }
}
