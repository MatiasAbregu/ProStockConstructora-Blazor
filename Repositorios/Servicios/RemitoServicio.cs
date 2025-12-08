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
using DTO.DTOs_Usuarios;

namespace Repositorios.Servicios
{
    public class RemitoServicio : IRemitoServicio
    {
        private readonly AppDbContext baseDeDatos;
        private readonly IMovimientoServicio movimientoServicio;

        public RemitoServicio(AppDbContext BaseDeDatos, IMovimientoServicio movimientoServicio)
        {
            baseDeDatos = BaseDeDatos;
            this.movimientoServicio = movimientoServicio;
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

        public async Task<Response<NotaDePedidoParaRemitoDTO>> ObtenerNotaDePedidoParaRemito(long NotaDePedidoId, long DepositoId)
        {
            try
            {
                var notaDePedido = await baseDeDatos.NotaDePedidos.FirstOrDefaultAsync(np => np.Id == NotaDePedidoId);
                if (notaDePedido == null)
                    return new Response<NotaDePedidoParaRemitoDTO>()
                    {
                        Estado = true,
                        Mensaje = "La nota de pedido no existe.",
                        Objeto = null
                    };

                var deposito = await baseDeDatos.Depositos.FirstOrDefaultAsync(d => d.Id == DepositoId);
                if (deposito == null)
                    return new Response<NotaDePedidoParaRemitoDTO>()
                    {
                        Estado = true,
                        Mensaje = "El depósito no existe.",
                        Objeto = null
                    };

                var detalles = await baseDeDatos.DetalleNotaDePedidos.Include(dnp => dnp.Recurso).Where(dnp =>
                    dnp.NotaDePedidoId == notaDePedido.Id && dnp.DepositoDestinoId == DepositoId).ToListAsync();
                var detallesnpids = detalles.Select(d => d.Id).ToList();
                var detallesr = await baseDeDatos.DetalleRemitos
                    .Where(dr => detallesnpids.Contains(dr.DetalleNotaDePedidoId)).ToListAsync();

                Dictionary<long, int> cantidadesDespachadas = new Dictionary<long, int>();
                foreach (var dr in detallesr)
                {
                    if (cantidadesDespachadas.ContainsKey(dr.DetalleNotaDePedidoId))
                    {
                        if (dr.Estado == EnumEstadoRemito.Recibido)
                            cantidadesDespachadas[dr.DetalleNotaDePedidoId] += dr.CantidadRecibida ?? 0;
                        else
                        {
                            cantidadesDespachadas[dr.DetalleNotaDePedidoId] += dr.CantidadDespachada;
                        }
                    }
                    else
                    {
                        if (dr.Estado == EnumEstadoRemito.Recibido)
                            cantidadesDespachadas[dr.DetalleNotaDePedidoId] = dr.CantidadRecibida ?? 0;
                        else
                        {
                            cantidadesDespachadas[dr.DetalleNotaDePedidoId] = dr.CantidadDespachada;
                        }
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
                            CantidadDespachada =
                                cantidadesDespachadas.ContainsKey(d.Id) ? cantidadesDespachadas[d.Id] : 0,
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

        public async Task<Response<List<VerRemitoDTO>>> ObtenerRemitosPorNotaDePedido(long NotaDePedidoId, long DepositoId)
        {
            try
            {
                var remitos = await baseDeDatos.Remitos.Where(r => r.NotaDePedidoId == NotaDePedidoId).ToListAsync();

                if (remitos.Count == 0)
                    return new Response<List<VerRemitoDTO>>()
                    {
                        Estado = true,
                        Mensaje = "¡No hay remitos aún generados para esta nota de pedido!",
                        Objeto = null
                    };

                var DetallesRemito = await baseDeDatos.DetalleRemitos.Include(dr => dr.DetalleNotaDePedido)
                    .Where(dr => remitos.Select(r => r.Id).Contains(dr.RemitoId) && dr.Estado != EnumEstadoRemito.Anulado
                        && dr.DetalleNotaDePedido.DepositoDestinoId == DepositoId)
                    .ToListAsync();

                var RemitosIds = DetallesRemito.Select(dr => dr.RemitoId).Distinct();
                remitos = remitos.Where(r => RemitosIds.Contains(r.Id)).ToList();
                var remitosParaElFront = remitos.Select(r => new VerRemitoDTO
                {
                    Id = r.Id,
                    NumeroRemito = r.NumeroRemito,
                    FechaEmision = r.FechaEmision,
                    Estado = DefinirEstadoRemito(DetallesRemito.Where(d => d.RemitoId == r.Id).ToList())
                }).ToList();

                if (remitosParaElFront.Count == 0)
                    return new Response<List<VerRemitoDTO>>()
                    {
                        Estado = true,
                        Mensaje = "¡No hay remitos aún generados para esta nota de pedido!",
                        Objeto = null
                    };

                return new Response<List<VerRemitoDTO>>
                {
                    Estado = true,
                    Mensaje = null,
                    Objeto = remitosParaElFront
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return new Response<List<VerRemitoDTO>>()
                {
                    Estado = false,
                    Mensaje = "¡Hubo un error al obtener los remitos asociados a la nota de pedido!",
                    Objeto = null
                };
            }
        }

        public async Task<Response<List<VerRemitoDTO>>> ObtenerRemitosPendietesPorNotaDePedido(long NotaDePedidoId)
        {
            try
            {
                var remitos = await baseDeDatos.Remitos.Where(r => r.NotaDePedidoId == NotaDePedidoId).ToListAsync();

                if (remitos.Count > 0)
                {
                    var DetallesRemitos = await baseDeDatos.DetalleRemitos
                        .Where(dr => remitos.Select(r => r.Id).Contains(dr.RemitoId)) .ToListAsync();

                    return new Response<List<VerRemitoDTO>>
                    {
                        Estado = true,
                        Mensaje = null,
                        Objeto = remitos.Select(r => new VerRemitoDTO()
                        {
                            Id = r.Id,
                            NumeroRemito = r.NumeroRemito,
                            FechaEmision = r.FechaEmision,
                            Estado = DefinirEstadoRemito(DetallesRemitos.Where(d => d.RemitoId == r.Id).ToList()),
                        }).ToList()
                    };
                }

                return new Response<List<VerRemitoDTO>>
                {
                    Estado = true,
                    Mensaje = "¡Aún no hay remitos pendientes que aceptar!.",
                    Objeto = null,
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return new Response<List<VerRemitoDTO>>()
                {
                    Estado = false,
                    Mensaje = "¡Hubo un error al cargar los remitos pendientes!",
                    Objeto = null
                };
            }
        }

        public async Task<Response<List<VerRemitoDTO>>> ObtenerRemitosPendientes(DatosUsuario Usuario)
        {
            try
            {
                var usuarioExiste =
                    await baseDeDatos.Usuarios.AnyAsync(u => u.Id == Usuario.Id && u.Email == Usuario.Email);
                if (!usuarioExiste)
                    return new Response<List<VerRemitoDTO>>()
                    {
                        Estado = true,
                        Mensaje = "Acceso denegado.",
                        Objeto = null
                    };

                List<Remito> remitos;
                if (Usuario.Roles.Contains("ADMINISTRADOR"))
                {
                    var existe = await baseDeDatos.Empresa.AnyAsync(e => e.Id == Usuario.EmpresaId);

                    if (!existe)
                        return new Response<List<VerRemitoDTO>>()
                        {
                            Estado = true,
                            Mensaje = "No existe una empresa con ese ID.",
                            Objeto = null
                        };

                    remitos = await baseDeDatos.DetalleRemitos
                        .Include(dr => dr.DetalleNotaDePedido)
                        .ThenInclude(dnp => dnp.DepositoDestino)
                        .ThenInclude(d => d.Obra)
                        .Where(dr =>
                            dr.DetalleNotaDePedido.DepositoDestino.Obra.EmpresaId == Usuario.EmpresaId &&
                            dr.Estado == EnumEstadoRemito.Emitido)
                        .Select(dr => dr.Remito).Distinct().ToListAsync();

                    if (remitos.Count > 0)
                    {
                        var DetallesRemitos = await baseDeDatos.DetalleRemitos
                            .Where(dr => remitos.Select(r => r.Id).Contains(dr.RemitoId))
                            .ToListAsync();

                        return new Response<List<VerRemitoDTO>>
                        {
                            Estado = true,
                            Mensaje = null,
                            Objeto = remitos.Select(r => new VerRemitoDTO()
                            {
                                Id = r.Id,
                                NumeroRemito = r.NumeroRemito,
                                FechaEmision = r.FechaEmision,
                                Estado = DefinirEstadoRemito(DetallesRemitos.Where(d => d.RemitoId == r.Id).ToList()),
                            }).ToList()
                        };
                    }
                }
                else if (Usuario.Roles.Contains("JEFEDEDEPOSITO"))
                {
                    remitos = await baseDeDatos.DetalleRemitos
                        .Include(dr => dr.DetalleNotaDePedido)
                        .Where(dr => Usuario.DepositosId.Contains(dr.DetalleNotaDePedido.DepositoDestinoId) &&
                                     dr.Estado == EnumEstadoRemito.Emitido).Select(dr => dr.Remito)
                        .Distinct().ToListAsync();

                    if (remitos.Count > 0)
                    {
                        var DetallesRemitos = await baseDeDatos.DetalleRemitos
                            .Where(dr => remitos.Select(r => r.Id).Contains(dr.RemitoId))
                            .ToListAsync();

                        return new Response<List<VerRemitoDTO>>
                        {
                            Estado = true,
                            Mensaje = null,
                            Objeto = remitos.Select(r => new VerRemitoDTO()
                            {
                                Id = r.Id,
                                NumeroRemito = r.NumeroRemito,
                                FechaEmision = r.FechaEmision,
                                Estado = DefinirEstadoRemito(DetallesRemitos.Where(d => d.RemitoId == r.Id).ToList()),
                            }).ToList()
                        };
                    }
                }

                return new Response<List<VerRemitoDTO>>
                {
                    Estado = true,
                    Mensaje = "¡Estás al día! No hay remitos pendientes.",
                    Objeto = null,
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return new Response<List<VerRemitoDTO>>()
                {
                    Estado = false,
                    Mensaje = "¡Hubo un error al cargar los remitos pendientes!",
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

                if (remitoDTO.DetallesDelRemito.All(r => r.CantidadDespachada == 0))
                    return new Response<string>()
                    {
                        Estado = false,
                        Mensaje = "El remito no puede tener en todos los renglones cantidad nula.",
                        Objeto = null
                    };

                var remito = new Remito()
                {
                    NumeroRemito = remitoDTO.NumeroRemito,
                    NotaDePedidoId = remitoDTO.NotaDePedidoId,
                    DepositoOrigenId = remitoDTO.DepositoOrigenId,
                    UsuarioId = remitoDTO.UsuarioId,
                    FechaEmision = DateTime.Now
                };
                baseDeDatos.Remitos.Add(remito);
                await baseDeDatos.SaveChangesAsync();

                List<DetalleRemito> detallesRemito = new();
                List<MovimientoStock> movimientosStock = new();

                var stocks = await baseDeDatos.Stocks
                    .Where(s => s.DepositoId == remitoDTO.DepositoOrigenId)
                    .ToListAsync();

                foreach (var detalleDTO in remitoDTO.DetallesDelRemito)
                {
                    if (detalleDTO.CantidadDespachada > 0)
                    {
                        var detalleRemito = new DetalleRemito()
                        {
                            RemitoId = remito.Id,
                            DetalleNotaDePedidoId = detalleDTO.DetalleNotaDePedidoId,
                            CantidadDespachada = detalleDTO.CantidadDespachada,
                            Estado = EnumEstadoRemito.Emitido
                        };

                        detallesRemito.Add(detalleRemito);
                        var stock = stocks.FirstOrDefault(s => s.RecursoId == detalleDTO.RecursoId);

                        var movimiento = new MovimientoStock()
                        {
                            DetalleRemito = detalleRemito,
                            StockId = stock.Id,
                            Cantidad = detalleDTO.CantidadDespachada,
                            TipoDeMovimiento = TipoDeMovimiento.Egreso,
                            Fecha = DateTime.Now
                        };

                        movimientosStock.Add(movimiento);
                    }
                }

                baseDeDatos.DetalleRemitos.AddRange(detallesRemito);
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
                Console.WriteLine("Error: " + e.StackTrace);
                return new Response<string>()
                {
                    Estado = false,
                    Mensaje = "¡Hubo un error al crear el remito!",
                    Objeto = null
                };
            }
        }

        public async Task<Response<VerRemitoDetalladoDTO>> ObtenerDetallesRemitoPorId(long RemitoId)
        {
            try
            {
                var remito = await baseDeDatos.Remitos
                    .Include(r => r.Usuario).Include(r => r.DepositoOrigen)
                    .FirstOrDefaultAsync(r => r.Id == RemitoId);

                if (remito == null)
                {
                    return new Response<VerRemitoDetalladoDTO>
                    {
                        Estado = true,
                        Mensaje = "El remito no existe.",
                        Objeto = null
                    };
                }

                var detalles = await baseDeDatos.DetalleRemitos
                    .Include(dr => dr.DetalleNotaDePedido).ThenInclude(dnp => dnp.Recurso)
                    .Include(dr => dr.UsuarioQueRecibe)
                    .Where(dr => dr.RemitoId == RemitoId)
                    .ToListAsync();

                var verRemitoDTO = new VerRemitoDetalladoDTO()
                {
                    Id = remito.Id,
                    NumeroRemito = remito.NumeroRemito,
                    DepositoOrigen = $"{remito.DepositoOrigen.NombreDeposito} ({remito.DepositoOrigen.CodigoDeposito})",
                    Usuario = $"{remito.Usuario.NombreUsuario} ({remito.Usuario.Email})",
                    FechaEmision = remito.FechaEmision,
                    Detalles = detalles.Select(d => new VerDetalleRemitoDTO()
                    {
                        Id = d.Id,
                        RemitoId = remito.Id,
                        DetalleNotaDePedidoId =  d.DetalleNotaDePedidoId,
                        Recurso = $"{d.DetalleNotaDePedido.Recurso.Nombre} ({d.DetalleNotaDePedido.Recurso.CodigoISO})",
                        Cantidad = d.DetalleNotaDePedido.Cantidad,
                        CantidadDespachada = d.CantidadDespachada,
                        CantidadRecibida = d.CantidadRecibida ?? 0,
                        Estado = (DTO.Enum.EnumEstadoRemito)d.Estado,
                        UsuarioQueRecibe = d.UsuarioQueRecibe != null
                            ? $"{d.UsuarioQueRecibe.NombreUsuario} ({d.UsuarioQueRecibe.Email})"
                            : "",
                    }).ToList()
                };

                return new Response<VerRemitoDetalladoDTO>
                {
                    Estado = true,
                    Mensaje = null,
                    Objeto = verRemitoDTO
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return new Response<VerRemitoDetalladoDTO>()
                {
                    Estado = false,
                    Mensaje = "",
                    Objeto = null
                };
            }
        }

        public async Task<Response<string>> ActualizarEstadosRemito(long RemitoId, List<VerDetalleRemitoDTO> detalles)
        {
            try
            {
                using var transaccion = await baseDeDatos.Database.BeginTransactionAsync();
                var listadoIds = detalles.Select(d => d.Id);
                var incosistencia = await baseDeDatos.DetalleRemitos
                    .AnyAsync(d => listadoIds.Contains(d.Id) && d.RemitoId != RemitoId);

                if (incosistencia)
                    return new Response<string>()
                    {
                        Estado = true,
                        Mensaje = "Un detalle no coincide con su remito ID.",
                        Objeto = null
                    };

                var detallesBBDD = await baseDeDatos.DetalleRemitos
                    .Where(d => listadoIds.Contains(d.Id) && d.RemitoId == RemitoId).ToListAsync();

                var dicDetalles = detalles.ToDictionary(d => d.Id);
                var movimientos = await baseDeDatos.MovimientoStocks.Include(m => m.Stock)
                    .Where(m => detallesBBDD.Select(d => d.Id).Contains(m.DetalleRemitoId)).ToListAsync();
                var depositoDestinoValidacion = await baseDeDatos.DetalleNotaDePedidos
                    .Where(dnp => detalles.Select(d => d.DetalleNotaDePedidoId).Contains(dnp.Id))
                    .Select(dnp => dnp.NotaDePedido).Distinct().Select(np => np.DepositoOrigen).ToListAsync();

                Console.WriteLine(depositoDestinoValidacion.Count);
                
                if (depositoDestinoValidacion.Count != 1)
                    return new Response<string>()
                    {
                        Estado = true,
                        Mensaje = "Hay incosistencia en la base de datos.",
                        Objeto = null
                    };

                var depositoDestino = depositoDestinoValidacion.First();

                foreach (var d in detallesBBDD)
                {
                    d.Estado = d.CantidadDespachada == dicDetalles[d.Id].CantidadRecibida
                        ? EnumEstadoRemito.Recibido
                        : EnumEstadoRemito.ParcialmenteRecibido;
                    d.CantidadRecibida = dicDetalles[d.Id].CantidadRecibida;
                    d.UsuarioQueRecibeId = dicDetalles[d.Id].UsuarioQueRecibeId;

                    var movimiento = movimientos.LastOrDefault(m => m.DetalleRemitoId == d.Id && m.TipoDeMovimiento == TipoDeMovimiento.Egreso);
                    await movimientoServicio.MovimientoStockEntreDepositos(depositoDestino, movimiento, d.Estado, dicDetalles[d.Id].CantidadRecibida ?? 0);
                }
                await baseDeDatos.SaveChangesAsync();
                await transaccion.CommitAsync();
                
                return new Response<string>()
                {
                    Estado = true,
                    Mensaje = null,
                    Objeto = "¡Estados actualizados con éxito!"
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Erorr: " + e.Message);
                return new Response<string>()
                {
                    Estado = false,
                    Mensaje = "¡Hubo un error al actualizar los estados del remito!",
                    Objeto = null
                };
            }
        }

        public async Task<Response<string>> AnularRemito(long RemitoId, long UsuarioId)
        {
            try
            {
                var remito = await baseDeDatos.Remitos.FirstOrDefaultAsync(r => r.Id == RemitoId);

                if (remito == null)
                    return new Response<string>()
                    {
                        Estado = true,
                        Mensaje = "El remito no existe",
                        Objeto = null
                    };

                var detalles = await baseDeDatos.DetalleRemitos
                    .Where(dr => dr.RemitoId == dr.Id).ToListAsync();

                var estados = detalles.Select(d => d.Estado);

                if (estados.Any(e => e != EnumEstadoRemito.Emitido))
                    return new Response<string>()
                    {
                        Estado = true,
                        Mensaje = "El remito ya no se puede anular porque un detalle ya fue modificado.",
                        Objeto = null
                    };

                foreach (var detalle in detalles)
                {
                    detalle.Estado = EnumEstadoRemito.Anulado;
                    detalle.UsuarioQueRecibeId = UsuarioId;
                }

                await baseDeDatos.SaveChangesAsync();

                List<MovimientoStock> movimientosStock = [];
                foreach (var detalle in detalles)
                {
                    var movimientoStock = new MovimientoStock()
                    {
                        DetalleRemitoId = detalle.Id,
                        Cantidad = detalle.CantidadDespachada,
                        TipoDeMovimiento = TipoDeMovimiento.Cancelado,
                        Fecha = DateTime.Now
                    };
                    movimientosStock.Add(movimientoStock);
                }

                baseDeDatos.MovimientoStocks.AddRange(movimientosStock);
                await baseDeDatos.SaveChangesAsync();

                return new Response<string>()
                {
                    Estado = true,
                    Mensaje = null,
                    Objeto = "¡El remito fue anulado con éxito!"
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return new Response<string>()
                {
                    Estado = false,
                    Mensaje = "¡Hubo un error al anular el remito!" + (e.Message.Contains("Cantidad de stock insuficiente") ? $" Causa: {e.Message}" : ""),
                    Objeto = null
                };
            }
        }

        // Métodos INTERNOS
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

        private DTO.Enum.EnumEstadoRemito DefinirEstadoRemito(List<DetalleRemito> detalles)
        {
            var Estados = detalles.Select(r => r.Estado).ToList();

            var recibidos = detalles.Count(d => d.Estado == (EnumEstadoRemito)EnumEstadoRemito.Recibido);
            var parcialmenteRecibido =
                detalles.Count(d => d.Estado == (EnumEstadoRemito)EnumEstadoRemito.ParcialmenteRecibido);
            var emitido = detalles.Count(d => d.Estado == (EnumEstadoRemito)EnumEstadoRemito.Emitido);

            if (recibidos == detalles.Count)
                return DTO.Enum.EnumEstadoRemito.Recibido;

            if (parcialmenteRecibido == detalles.Count)
                return DTO.Enum.EnumEstadoRemito.ParcialmenteRecibido;

            if (emitido == detalles.Count)
                return DTO.Enum.EnumEstadoRemito.Emitido;

            if (parcialmenteRecibido > 0)
                return DTO.Enum.EnumEstadoRemito.ParcialmenteRecibido;

            return DTO.Enum.EnumEstadoRemito.Emitido;
        }
        
    }
}