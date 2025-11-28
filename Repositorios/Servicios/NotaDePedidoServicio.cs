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
using DTO.DTOs_Response;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using DTO.Enum;
using BD.Enums;

namespace Repositorios.Servicios
{
    public class NotaDePedidoServicio : INotaDePedidoServicio
    {
        private readonly AppDbContext BasedeDatos;

        public NotaDePedidoServicio(AppDbContext BasedeDatos)
        {
            this.BasedeDatos = BasedeDatos;

        }


        private async Task<string> GenerarNumeroNotaPedidoAsync()
        {
            var ultimoNumero = await BasedeDatos.NotaDePedidos
                .OrderByDescending(np => np.Id)
                .Select(np => np.Id)
                .FirstOrDefaultAsync();
            string numeroNotaPedido = $"NP-";
            if (ultimoNumero != null && ultimoNumero != 0)
            {
                numeroNotaPedido += $"{ultimoNumero + 1}";
            }
            else
            {
                numeroNotaPedido += "1";
            }
            return numeroNotaPedido;
        }

        public async Task<Response<string>> ObtenerNumeroNotadePedidoSiguiente()
        {
            try
            {
                return new Response<string>
                {
                    Estado = true,
                    Mensaje = null,
                    Objeto = await GenerarNumeroNotaPedidoAsync()
                };
            }
            catch (Exception e)
            {

                Console.WriteLine("Error" + e.Message);
                return new Response<string>
                {
                    Estado = false,
                    Mensaje = "Error al obtener el número de nota de pedido.",
                    Objeto = null
                };
            }
        }

        public async Task<Response<string>> CrearNotaDePedido(CrearNotaDePedidoDTO NotadePedidoDTO)
        {
            try
            {
                if (NotadePedidoDTO.ListaDePedido.Count <= 0)
                    return new Response<string>
                    {
                        Estado = true,
                        Mensaje = "La nota de pedido no tiene renglones",
                        Objeto = null
                    };
                var NotadePedido = new NotaDePedido()
                {
                    DepositoOrigenId = NotadePedidoDTO.DepositoOrigenId,
                    FechaEmision = DateTime.Now,
                    NumeroNotaPedido = await GenerarNumeroNotaPedidoAsync(),
                    UsuarioId = NotadePedidoDTO.UsuarioId,
                };
                BasedeDatos.Add(NotadePedido);
                await BasedeDatos.SaveChangesAsync();
                List<DetalleNotaDePedido> Detalles = new();

                foreach (var DetalleDTO in NotadePedidoDTO.ListaDePedido)
                {
                    var Detalle = new DetalleNotaDePedido()
                    {
                        Cantidad = DetalleDTO.Cantidad,
                        DepositoDestinoId = DetalleDTO.DepositoDestinoId,
                        RecursoId = DetalleDTO.RecursoId,
                        NotaDePedidoId = NotadePedido.Id,


                    };
                    Detalles.Add(Detalle);
                }
                BasedeDatos.AddRange(Detalles);
                await BasedeDatos.SaveChangesAsync();
                return new Response<string>
                {
                    Estado = true,
                    Mensaje = null,
                    Objeto = "La nota de pedido se ha creado correctamente."
                };

            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e.Message);
                return new Response<string>
                {
                    Estado = false,
                    Mensaje = "Error al crear la nota de pedido.",
                    Objeto = null
                };
            }
        }
        public async Task<Response<List<VerNotaDePedidoDTO>>> ObtenerNotasDePedidoPorDepositoId(long DepositoId)
        {
            try
            {
                var notasDePedido = await BasedeDatos.NotaDePedidos
                    .Where(np => np.DepositoOrigenId == DepositoId)
                    .ToListAsync();

                if (notasDePedido.Count > 0)
                {
                    var DetallesNotaDePedido = await BasedeDatos.DetalleNotaDePedidos
                        .Where(dnp => notasDePedido.Select(np => np.Id).Contains(dnp.NotaDePedidoId))
                        .ToListAsync();

                    return new Response<List<VerNotaDePedidoDTO>>
                    {
                        Estado = true,
                        Mensaje = null,
                        Objeto = notasDePedido.Select(np => new VerNotaDePedidoDTO
                        {
                            Id = np.Id,
                            NumeroNotaPedido = np.NumeroNotaPedido,
                            FechaEmision = np.FechaEmision,
                            Estado =  DefinirEstadoNotaPedido(DetallesNotaDePedido.Where(d => d.NotaDePedidoId == np.Id).ToList()),

                        }).ToList()
                    };
                }
                else
                {
                    return new Response<List<VerNotaDePedidoDTO>>
                    {
                        Estado = true,
                        Mensaje = "No hay notas de pedido para el depósito especificado.",
                        Objeto = null,
                    };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e.Message);
                return new Response<List<VerNotaDePedidoDTO>>
                {
                    Estado = false,
                    Mensaje = "Error al obtener nota de pedido por deposito.",
                    Objeto = null,
                };


            }
        }
        private EnumEstadoNotaPedido DefinirEstadoNotaPedido(List<DetalleNotaDePedido> detalles)
        {
            var Estados = detalles.Select(d => d.EstadoNotaPedido).ToList();

            if (Estados.All(e => e == (EstadoNotaPedido)EnumEstadoNotaPedido.Aprobada))
                return EnumEstadoNotaPedido.Aprobada;
            else if (Estados.All(e => e == (EstadoNotaPedido)EnumEstadoNotaPedido.Rechazada))
                return EnumEstadoNotaPedido.Rechazada;
            else if (Estados.Any(e=> e == (EstadoNotaPedido)EnumEstadoNotaPedido.Aprobada))
                return EnumEstadoNotaPedido.ParcialmenteAprobada;
            else if (Estados.Any(e => e == (EstadoNotaPedido)EnumEstadoNotaPedido.Rechazada))
                return EnumEstadoNotaPedido.ParcialmenteRechazada;
            else
                return EnumEstadoNotaPedido.Pendiente;
        }
        public async Task<Response<VerNotaDePedidoDTO>> ObtenerDetallesNotaDePedidoPorId(long NotaDePedidoId)
        {
            try
            {
                var notaDePedido = await BasedeDatos.NotaDePedidos
                    .FirstOrDefaultAsync(np => np.Id == NotaDePedidoId);
                if (notaDePedido == null)
                {
                    return new Response<VerNotaDePedidoDTO>
                    {
                        Estado = true,
                        Mensaje = "La nota de pedido no existe.",
                        Objeto = null
                    };
                }
                var detalles = await BasedeDatos.DetalleNotaDePedidos
                    .Where(dnp => dnp.NotaDePedidoId == NotaDePedidoId)
                    .ToListAsync();
                var verNotaDePedidoDTO = new VerNotaDePedidoDTO
                {
                    Id = notaDePedido.Id,
                    NumeroNotaPedido = notaDePedido.NumeroNotaPedido,
                    FechaEmision = notaDePedido.FechaEmision,
                    Estado = DefinirEstadoNotaPedido(detalles),
                    ListaDePedido = detalles.Select(d => new DetalleNotaDePedidoDTO
                    {
                        Cantidad = d.Cantidad,
                        DepositoDestinoId = d.DepositoDestinoId,
                        RecursoId = d.RecursoId,
                        EstadoNotaPedido = (EnumEstadoNotaPedido)d.EstadoNotaPedido
                    }).ToList()
                };
                return new Response<VerNotaDePedidoDTO>
                {
                    Estado = true,
                    Mensaje = null,
                    Objeto = verNotaDePedidoDTO
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e.Message);
                return new Response<VerNotaDePedidoDTO>
                {
                    Estado = false,
                    Mensaje = "Error al obtener los detalles de la nota de pedido.",
                    Objeto = null
                };
            }
        }
    }
}
