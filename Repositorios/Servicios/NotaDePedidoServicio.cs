using BD;
using BD.Enums;
using BD.Modelos;
using DTO.DTOs_NotaDePedido;
using DTO.DTOs_Remitos;
using DTO.DTOs_Response;
using DTO.DTOs_Usuarios;
using DTO.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Repositorios.Implementaciones;
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
        private readonly AppDbContext BasedeDatos;

        public NotaDePedidoServicio(AppDbContext BasedeDatos)
        {
            this.BasedeDatos = BasedeDatos;
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
                        .Where(dnp => notasDePedido.Select(np => np.Id).Contains(dnp.NotaDePedidoId) && dnp.EstadoNotaPedido != EstadoNotaPedido.Anulada)
                        .ToListAsync();

                    var NotasDePedidoIds = DetallesNotaDePedido.Select(dnp => dnp.NotaDePedidoId).Distinct();
                    notasDePedido = notasDePedido.Where(np => NotasDePedidoIds.Contains(np.Id)).ToList();
                    var notasDePedidoParaElFront = notasDePedido.Select(np => new VerNotaDePedidoDTO
                    {
                        Id = np.Id,
                        NumeroNotaPedido = np.NumeroNotaPedido,
                        FechaEmision = np.FechaEmision,
                        Estado = DefinirEstadoNotaPedido(DetallesNotaDePedido.Where(d => d.NotaDePedidoId == np.Id).ToList()),
                    }).ToList();

                    if (notasDePedidoParaElFront.Count == 0)
                        return new Response<List<VerNotaDePedidoDTO>>()
                        {
                            Estado = true,
                            Mensaje = "¡No hay notas de pedido cargadas aún en este depósito!",
                            Objeto = null
                        };
                    
                    return new Response<List<VerNotaDePedidoDTO>>
                    {
                        Estado = true,
                        Mensaje = null,
                        Objeto = notasDePedidoParaElFront
                    };
                }
                else
                {
                    return new Response<List<VerNotaDePedidoDTO>>
                    {
                        Estado = true,
                        Mensaje = "¡No hay notas de pedido cargadas aún en este depósito!",
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

        public async Task<Response<VerNotadePedidoDetalladaDTO>> ObtenerDetallesNotaDePedidoPorId(long NotaDePedidoId)
        {
            try
            {
                var notaDePedido = await BasedeDatos.NotaDePedidos
                    .Include(np => np.Usuario).Include(np => np.DepositoOrigen)
                    .FirstOrDefaultAsync(np => np.Id == NotaDePedidoId);
                if (notaDePedido == null)
                {
                    return new Response<VerNotadePedidoDetalladaDTO>
                    {
                        Estado = true,
                        Mensaje = "La nota de pedido no existe.",
                        Objeto = null
                    };
                }
                var detalles = await BasedeDatos.DetalleNotaDePedidos
                    .Include(dnp => dnp.DepositoDestino).Include(dnp => dnp.Recurso)
                    .Include(dnp => dnp.UsuarioModificacion)
                    .Where(dnp => dnp.NotaDePedidoId == NotaDePedidoId)
                    .ToListAsync();

                var verNotaDePedidoDTO = new VerNotadePedidoDetalladaDTO
                {
                    Id = notaDePedido.Id,
                    NumeroNotaPedido = notaDePedido.NumeroNotaPedido,
                    FechaEmision = notaDePedido.FechaEmision,
                    DepositoOrigen = $"{notaDePedido.DepositoOrigen.NombreDeposito} ({notaDePedido.DepositoOrigen.CodigoDeposito})",
                    Usuario = $"{notaDePedido.Usuario.NombreUsuario} ({notaDePedido.Usuario.Email})",
                    Detalles = detalles.Select(d => new VerDetalleNotadePedidoDTO
                    {
                        Id = d.Id,
                        RecursoId = d.RecursoId,
                        Recurso = $"{d.Recurso.Nombre} ({d.Recurso.CodigoISO})",
                        Cantidad = d.Cantidad,
                        DepositoId = d.DepositoDestinoId,
                        Deposito = $"{d.DepositoDestino.NombreDeposito} ({d.DepositoDestino.CodigoDeposito})",
                        Estado = (EnumEstadoNotaPedido)d.EstadoNotaPedido,
                        UsuarioQueModifico = d.UsuarioModificacion != null ? $"{d.UsuarioModificacion.NombreUsuario} ({d.UsuarioModificacion.Email})" : ""
                    }).ToList()
                };
                return new Response<VerNotadePedidoDetalladaDTO>
                {
                    Estado = true,
                    Mensaje = null,
                    Objeto = verNotaDePedidoDTO
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e.Message);
                return new Response<VerNotadePedidoDetalladaDTO>
                {
                    Estado = false,
                    Mensaje = "Error al obtener los detalles de la nota de pedido.",
                    Objeto = null
                };
            }
        }

        public async Task<Response<List<VerNotaDePedidoDTO>>> ObtenerNotasDePedidoPendientes(DatosUsuario Usuario)
        {
            try
            {
                var usuarioExiste = await BasedeDatos.Usuarios.AnyAsync(u => u.Id == Usuario.Id && u.Email == Usuario.Email);
                if (!usuarioExiste) return new Response<List<VerNotaDePedidoDTO>>()
                {
                    Estado = true,
                    Mensaje = "Acceso denegado.",
                    Objeto = null
                };

                List<NotaDePedido> notasDePedido;
                if (Usuario.Roles.Contains("ADMINISTRADOR"))
                {
                    var existe = await BasedeDatos.Empresa.AnyAsync(e => e.Id == Usuario.EmpresaId);

                    if (!existe) return new Response<List<VerNotaDePedidoDTO>>()
                    {
                        Estado = true,
                        Mensaje = "No existe una empresa con ese ID.",
                        Objeto = null
                    };

                    notasDePedido = await BasedeDatos.DetalleNotaDePedidos
                        .Include(dnp => dnp.DepositoDestino).ThenInclude(d => d.Obra)
                    .Where(np => np.DepositoDestino.Obra.EmpresaId == Usuario.EmpresaId && np.EstadoNotaPedido == EstadoNotaPedido.Pendiente)
                    .Select(dnp => dnp.NotaDePedido).Distinct().ToListAsync();

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
                                Estado = DefinirEstadoNotaPedido(DetallesNotaDePedido.Where(d => d.NotaDePedidoId == np.Id).ToList()),
                            }).ToList()
                        };
                    }
                }
                else if (Usuario.Roles.Contains("JEFEDEDEPOSITO"))
                {
                    notasDePedido = await BasedeDatos.DetalleNotaDePedidos
                   .Where(np => Usuario.DepositosId.Contains(np.DepositoDestinoId) && np.EstadoNotaPedido == EstadoNotaPedido.Pendiente).Select(dnp => dnp.NotaDePedido)
                   .Distinct().ToListAsync();

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
                                Estado = DefinirEstadoNotaPedido(DetallesNotaDePedido.Where(d => d.NotaDePedidoId == np.Id).ToList()),
                            }).ToList()
                        };
                    }
                }

                return new Response<List<VerNotaDePedidoDTO>>
                {
                    Estado = true,
                    Mensaje = "¡Estás al día! No hay notas de pedido pendientes.",
                    Objeto = null,
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return new Response<List<VerNotaDePedidoDTO>>()
                {
                    Estado = false,
                    Mensaje = "¡Hubo un error al cargar las notas de pedido pendientes!",
                    Objeto = null
                };
            }
        }

        public async Task<Response<List<VerNotaDePedidoDTO>>> ObtenerNotasDePedidoPendientesPorDepositoId(long DepositoId)
        {
            try
            { 
                var notasDePedido = await BasedeDatos.DetalleNotaDePedidos
               .Where(np => np.DepositoDestinoId == DepositoId && np.EstadoNotaPedido != EstadoNotaPedido.Anulada)
               .Select(dnp => dnp.NotaDePedido).Distinct().ToListAsync();

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
                            Estado = DefinirEstadoNotaPedido(DetallesNotaDePedido.Where(d => d.NotaDePedidoId == np.Id).ToList()),
                        }).ToList()
                    };
                }

                return new Response<List<VerNotaDePedidoDTO>>
                {
                    Estado = true,
                    Mensaje = "¡El depósito está al día! No hay notas de pedido pendientes.",
                    Objeto = null,
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return new Response<List<VerNotaDePedidoDTO>>()
                {
                    Estado = false,
                    Mensaje = "¡Hubo un error al cargar las notas de pedido pendientes!",
                    Objeto = null
                };
            }
        }

        public async Task<Response<string>> ActualizarEstadosNotaDePedido(long NotaDePedidoId, List<VerDetalleNotadePedidoDTO> detalles)
        {
            try
            {
                var listadoIds = detalles.Select(d => d.Id);
                var incosistencia = await BasedeDatos.DetalleNotaDePedidos
                                    .AnyAsync(d => listadoIds.Contains(d.Id) && d.NotaDePedidoId != NotaDePedidoId);

                if (incosistencia)
                    return new Response<string>()
                    {
                        Estado = true,
                        Mensaje = "Un detalle no coincide con su nota de pedido ID.",
                        Objeto = null
                    };

                var detallesBBDD = await BasedeDatos.DetalleNotaDePedidos
                                    .Where(d => listadoIds.Contains(d.Id) && d.NotaDePedidoId == NotaDePedidoId).ToListAsync();

                var dicDetalles = detalles.ToDictionary(d => d.Id);

                foreach (var d in detallesBBDD)
                {
                    d.EstadoNotaPedido = (EstadoNotaPedido)dicDetalles[d.Id].Estado;
                    d.UsuarioModificacionId = dicDetalles[d.Id].UsuarioQueModificadoId;
                }

                await BasedeDatos.SaveChangesAsync();

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
                    Mensaje = "¡Hubo un error al actualizar los estados de la nota de pedido!",
                    Objeto = null
                };
            }
        }

        public async Task<Response<string>> AnularNotaDePedido(long NotaDePedidoId, long UsuarioId)
        {
            try
            {
                var notaDePedido = await BasedeDatos.NotaDePedidos.FirstOrDefaultAsync(np => np.Id == NotaDePedidoId);
                if (notaDePedido == null)
                    return new Response<string>()
                    {
                        Estado = true,
                        Mensaje = "La nota de pedido no existe",
                        Objeto = null
                    };
                
                var detalles = await BasedeDatos.DetalleNotaDePedidos
                    .Where(dnp => dnp.NotaDePedidoId == notaDePedido.Id).ToListAsync();

                var estados = detalles.Select(d => d.EstadoNotaPedido);

                if (estados.Any(e => e != EstadoNotaPedido.Pendiente))
                    return new Response<string>()
                    {
                        Estado = true,
                        Mensaje = "La nota de pedido ya no se puede anular porque un detalle ya fue modificado.",
                        Objeto = null
                    };

                foreach (var detalle in detalles)
                {
                    detalle.EstadoNotaPedido = EstadoNotaPedido.Anulada;
                    detalle.UsuarioModificacionId = UsuarioId;
                }

                await BasedeDatos.SaveChangesAsync();

                return new Response<string>()
                {
                    Estado = true,
                    Mensaje = null,
                    Objeto = "¡La nota de pedido fue anulada con éxito!"
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return new Response<string>()
                {
                    Estado = false,
                    Mensaje = "¡Hubo un error al anular la nota de pedido!",
                    Objeto = null
                };
            }
        }

        // Metódos internos
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

        private EnumEstadoNotaPedido DefinirEstadoNotaPedido(List<DetalleNotaDePedido> detalles)
        {
            var Estados = detalles.Select(d => d.EstadoNotaPedido).ToList();

            var aprobados = detalles.Count(d => d.EstadoNotaPedido == (EstadoNotaPedido)EnumEstadoNotaPedido.Aprobada);
            var rechazados = detalles.Count(d => d.EstadoNotaPedido == (EstadoNotaPedido)EnumEstadoNotaPedido.Rechazada);
            var pendientes = detalles.Count(d => d.EstadoNotaPedido == (EstadoNotaPedido)EnumEstadoNotaPedido.Pendiente);

            if (aprobados == detalles.Count)
                return EnumEstadoNotaPedido.Aprobada;

            if (rechazados == detalles.Count)
                return EnumEstadoNotaPedido.Rechazada;

            if (pendientes == detalles.Count)
                return EnumEstadoNotaPedido.Pendiente;

            if (aprobados > 0)
                return EnumEstadoNotaPedido.ParcialmenteAprobada;

            if (aprobados == 0 && rechazados > 0)
                return EnumEstadoNotaPedido.ParcialmenteRechazada;

            return EnumEstadoNotaPedido.Pendiente;
        }
    }
}