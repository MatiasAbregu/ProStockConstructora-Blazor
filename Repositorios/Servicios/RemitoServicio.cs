using BD;
using BD.Enums;
using BD.Modelos;
using DTO.DTOs_NotaDePedido;
using DTO.DTOs_Remitos;
using DTO.DTOs_Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositorios.Implementaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.Servicios
{
    public class RemitoServicio : IRemitoServicio
    {
        private readonly AppDbContext baseDeDatos;

        public RemitoServicio(AppDbContext BaseDeDatos)
        {
            baseDeDatos = BaseDeDatos;
        }
        public async Task<Response<string>> ObtenerNumeroRemitoSiguiente()
        {
            try
            {
                return new Response<string>
                {
                    Estado = true,
                    Mensaje = null,
                    Objeto = await GenerarNumeroRemitoAsync()
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

        private async Task<string> GenerarNumeroRemitoAsync()
        {
            var ultimoNumero = await baseDeDatos.Remitos
                .OrderByDescending(r => r.Id)
                .Select(r => r.Id)
                .FirstOrDefaultAsync();
            string numeroRemito = $"R-";
            if (ultimoNumero != null && ultimoNumero != 0)
            {
                numeroRemito += $"{ultimoNumero + 1}";
            }
            else
            {
                numeroRemito += "1";
            }
            return numeroRemito;
        }

        public async Task<Response<NotaDePedidoParaRemitoDTO>> ObtenerNotaDePedidoParaRemito(long NotaDePedidoId, long DepositoId)
        {
            try
            {
                var notaDePedido = await baseDeDatos.NotaDePedidos.FirstOrDefaultAsync(np => np.Id == NotaDePedidoId);
                if (notaDePedido == null) return new Response<NotaDePedidoParaRemitoDTO>()
                {
                    Estado = true,
                    Mensaje = "La nota de pedido no existe.",
                    Objeto = null
                };

                var deposito = await baseDeDatos.Depositos.FirstOrDefaultAsync(d => d.Id == DepositoId);
                if (deposito == null) return new Response<NotaDePedidoParaRemitoDTO>()
                {
                    Estado = true,
                    Mensaje = "El depósito no existe.",
                    Objeto = null
                };

                var detalles = await baseDeDatos.DetalleNotaDePedidos.Include(dnp => dnp.Recurso).Where(dnp => dnp.NotaDePedidoId == notaDePedido.Id && dnp.DepositoDestinoId == DepositoId).ToListAsync();
                var detallesnpids = detalles.Select(d => d.Id).ToList();
                var detallesr = await baseDeDatos.DetalleRemitos.Where(dr => detallesnpids.Contains(dr.DetalleNotaDePedidoId)).ToListAsync();
                Dictionary<long, int> cantidadesDespachadas = new Dictionary<long, int>();
                foreach (var dr in detallesr)
                {
                    if (cantidadesDespachadas.ContainsKey(dr.DetalleNotaDePedidoId))
                    {
                        if (dr.Estado == EnumEstadoRemito.Recibido)
                            cantidadesDespachadas[dr.DetalleNotaDePedidoId] += dr.CantidadRecibida ?? 0;
                        else
                        { cantidadesDespachadas[dr.DetalleNotaDePedidoId] += dr.CantidadDespachada; }
                    }
                    else
                    {
                        if (dr.Estado == EnumEstadoRemito.Recibido)
                            cantidadesDespachadas[dr.DetalleNotaDePedidoId] = dr.CantidadRecibida ?? 0;
                        else
                        { cantidadesDespachadas[dr.DetalleNotaDePedidoId] = dr.CantidadDespachada; }
                    }
                }
                return new Response<NotaDePedidoParaRemitoDTO>()
                {
                    Estado = true,
                    Mensaje = null,
                    Objeto = new NotaDePedidoParaRemitoDTO()
                    {
                        NotaPedidoId = notaDePedido.Id,
                        DepositoDestino = $"{deposito.NombreDeposito} ({deposito.CodigoDeposito})",
                        detalle = detalles.Select(d => new VerDetalleNotaDePedidoParaRemitoDTO()
                        {
                            Id = d.Id,
                            RecursoId = d.RecursoId,
                            Recurso = $"{d.Recurso.Nombre} ({d.Recurso.CodigoISO})",
                            Cantidad = d.Cantidad,
                            CantidadDespachada = cantidadesDespachadas.ContainsKey(d.Id) ? cantidadesDespachadas[d.Id] : 0,
                        }).ToList()
                    }
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return new Response<NotaDePedidoParaRemitoDTO>()
                {
                    Estado = false,
                    Mensaje = "¡Hubo un error al obtener los datos de la nota de pedido para el remito!",
                    Objeto = null
                };
            }
        }

        public async Task<Response<string>> CrearRemito(CrearRemitoDTO remitoDTO)
        {
            try
            {
                if (remitoDTO.DetallesDelRemito?.Count == 0)
                {
                    return new Response<string>()
                    {
                        Estado = false,
                        Mensaje = "El remito debe tener al menos un detalle.",
                        Objeto = null
                    };
                }
                var remito = new Remito()
                {
                    NumeroRemito = remitoDTO.NumeroRemito,
                    NotaDePedidoId = remitoDTO.NotaDePedidoId,
                    DepositoOrigenId = remitoDTO.DepositoOrigenId,
                    UsuarioId = remitoDTO.UsuarioId,
                    FechaEmision = remitoDTO.FechaEmision,

                };
                baseDeDatos.Remitos.Add(remito);
                await baseDeDatos.SaveChangesAsync();
                List<DetalleRemito> detallesRemito = new List<DetalleRemito>();
                List<MovimientoStock> movimientosStock = new List<MovimientoStock>();
                foreach (var detalleDTO in remitoDTO.DetallesDelRemito)
                {
                    var detalleRemito = new DetalleRemito()
                    {
                        RemitoId = remito.Id,
                        DetalleNotaDePedidoId = detalleDTO.DetalleNotaDePedidoId,
                        CantidadDespachada = detalleDTO.CantidadDespachada,
                        Estado = EnumEstadoRemito.EnTransito
                    };
                    detallesRemito.Add(detalleRemito);
                }
                baseDeDatos.DetalleRemitos.AddRange(detallesRemito);
                await baseDeDatos.SaveChangesAsync();
                foreach (var detalle in detallesRemito)
                {
                    var movimientoStock = new MovimientoStock()
                    {
                        DetalleRemitoId = detalle.Id,
                        Cantidad = detalle.CantidadDespachada,
                        TipoDeMovimiento = TipoDeMovimiento.Egreso,                        
                    };
                    movimientosStock.Add(movimientoStock);
                }
                baseDeDatos.MovimientoStocks.AddRange(movimientosStock);
                await baseDeDatos.SaveChangesAsync();
                return new Response<string>()
                {
                    Estado = true,
                    Mensaje = null,
                    Objeto = remito.NumeroRemito
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return new Response<string>()
                {
                    Estado = false,
                    Mensaje = "¡Hubo un error al crear el remito!",
                    Objeto = null
                };

            }
        }
    }

}
