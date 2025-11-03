using BD;
using BD.Modelos;
using DTO.DTOs_NotaDePedido;
using Repositorios.Implementaciones;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.Servicios
{
    public class NotaDePedidoServicio : INotaDePedidoServicio
    {
        private readonly AppDbContext context;

        public NotaDePedidoServicio(AppDbContext context)
        {
            this.context = context;
            
        }

        public async Task<List<VerNotaDePedidoDTO>> ObtenerNotasDePedido(int notaDePedido)
        {
            var notasDePedido = await context.NotaDePedidos.ToListAsync();
            var notasDePedidoDTOs = notasDePedido.Select(np => new VerNotaDePedidoDTO
            {
                NumeroNotaPedido = np.NumeroNotaPedido,
                DepositoDestinoId = np.DepositoDestinoId,
                FechaEmision = np.FechaEmision,
                Estado = (DTO.Enum.EnumEstadoNotaPedido)np.Estado,
                ListaDePedido = context.DetalleNotaDePedidos
                    .Where(dnp => dnp.NotaDePedidoId == np.Id)
                    .Select(dnp => new DetalleNotaDePedidoDTO
                    {
                        MaterialesyMaquinasId = dnp.MaterialesyMaquinasId,
                        Cantidad = dnp.Cantidad
                    }).ToList()

            }).ToList();
            return notasDePedidoDTOs;


        }
        public async Task<VerNotaDePedidoDTO?> ObtenerNotaDePedidoPorId(string NumeroNotaPedido)
        {
            var notaDePedido = await context.NotaDePedidos
                .FirstOrDefaultAsync(np => np.NumeroNotaPedido == NumeroNotaPedido);
            if (notaDePedido == null)
            {
                return null;
            }
            return new VerNotaDePedidoDTO
            {
                NumeroNotaPedido = notaDePedido.NumeroNotaPedido,
                DepositoDestinoId = notaDePedido.DepositoDestinoId,
                FechaEmision = notaDePedido.FechaEmision,
                Estado = (DTO.Enum.EnumEstadoNotaPedido)notaDePedido.Estado
            };

            
        }

        //public async Task<(bool,string)> CrearNotaDePedido(CrearNotaDePedidoDTO nuevaNotaDePedidoDTO)
        //{
        //   bool existeNotaDePedido = await context.NotaDePedidos
        //        .AnyAsync(np => np.NumeroNotaPedido == nuevaNotaDePedidoDTO.NumeroNotaPedido);
        //    if (existeNotaDePedido)
        //    {
        //        return (false, "Ya existe una nota de pedido con ese número.");
        //    }
        //    var nuevaNotaDePedido = new NotaDePedido
        //    {
        //        NumeroNotaPedido = nuevaNotaDePedidoDTO.NumeroNotaPedido,
        //        DepositoDestinoId = nuevaNotaDePedidoDTO.DepositoDestinoId,
        //        FechaEmision = nuevaNotaDePedidoDTO.FechaEmision,
        //        Estado = (BD.Enums.EstadoNotaPedido)nuevaNotaDePedidoDTO.Estado,
        //        UsuarioId = nuevaNotaDePedidoDTO.UsuarioId
        //    };
        //    context.NotaDePedidos.Add(nuevaNotaDePedido);
        //    await context.SaveChangesAsync();
        //    foreach (var detalleDTO in nuevaNotaDePedidoDTO.ListaDePedido)
        //    {
        //        if (detalleDTO.Cantidad <= 0)
        //        {
        //            return (false, "La cantidad en el detalle de la nota de pedido debe ser mayor que cero.");
        //        }
        //        if (!await context.MaterialesyMaquinas.AnyAsync(mm => mm.Id == detalleDTO.MaterialesyMaquinasId))
        //        {
        //            return (false, $"No existe material o máquina con ID {detalleDTO.MaterialesyMaquinasId}.");
        //        }
        //        if (detalleDTO.MaterialesyMaquinasId <= 0)
        //        {
        //            return (false, "El ID de Materiales y Máquinas debe ser un número positivo.");
        //        }
        //        if (nuevaNotaDePedido.Id <= 0)
        //        {
        //            return (false, "Error al crear la nota de pedido. ID inválido.");
        //        }
        //        if (await context.Stocks.AnyAsync(s => s.Cantidad < detalleDTO.Cantidad && s.DepositoId == nuevaNotaDePedido.DepositoDestinoId))
        //        {
        //            return (false, "No hay suficiente stock en el depósito destino para uno de los materiales o máquinas solicitados.");
        //        }

        //        var detalleNotaDePedido = new DetalleNotaDePedido
        //        {
        //            NotaDePedidoId = nuevaNotaDePedido.Id,
        //            MaterialesyMaquinasId= detalleDTO.MaterialesyMaquinasId,
        //            Cantidad = detalleDTO.Cantidad
        //        };
        //        context.DetalleNotaDePedidos.Add(detalleNotaDePedido);
        //    }
        //    await context.SaveChangesAsync();
        //    return (true, "La nota de pedido se creó exitosamente.");
        //}
        public async Task<(bool,string)> ActualizarNotaDePedido(ActualizarNotaDePedidoDTO actualizarNotaDePedidoDTO, int notaPedidoId)
        {
            throw new NotImplementedException();

        }
        public async Task<(bool,string)> EliminarNotaDePedido(int id)
        {
            throw new NotImplementedException();
        }
    }
}
