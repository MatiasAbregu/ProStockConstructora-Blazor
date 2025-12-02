using BD;
using BD.Enums;
using BD.Modelos;
using DTO.DTOs_NotaDePedido;
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
                var usuarioExiste = await BasedeDatos.Usuarios.AnyAsync(u => u.Id == u.Id && u.Email == u.Email);
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
                    .Where(np => np.DepositoDestino.Obra.EmpresaId == Usuario.EmpresaId)
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
                else if (Usuario.Roles.Contains("JEFEDEOBRA"))
                {
                    var DepositosId = await BasedeDatos.Depositos.Where(d => Usuario.ObrasId.Contains(d.ObraId))
                                                       .Select(d => d.Id).ToListAsync();

                    notasDePedido = await BasedeDatos.DetalleNotaDePedidos
                   .Where(np => DepositosId.Contains(np.DepositoDestinoId)).Select(dnp => dnp.NotaDePedido)
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
                else if (Usuario.Roles.Contains("JEFEDEDEPOSITO"))
                {
                    notasDePedido = await BasedeDatos.DetalleNotaDePedidos
                   .Where(np => Usuario.DepositosId.Contains(np.DepositoDestinoId)).Select(dnp => dnp.NotaDePedido)
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
               .Where(np => np.DepositoDestinoId == DepositoId).Select(dnp => dnp.NotaDePedido)
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

            if (Estados.All(e => e == (EstadoNotaPedido)EnumEstadoNotaPedido.Aprobada))
                return EnumEstadoNotaPedido.Aprobada;
            else if (Estados.All(e => e == (EstadoNotaPedido)EnumEstadoNotaPedido.Rechazada))
                return EnumEstadoNotaPedido.Rechazada;
            else if (Estados.Any(e => e == (EstadoNotaPedido)EnumEstadoNotaPedido.Aprobada))
                return EnumEstadoNotaPedido.ParcialmenteAprobada;
            else if (Estados.Any(e => e == (EstadoNotaPedido)EnumEstadoNotaPedido.Rechazada))
                return EnumEstadoNotaPedido.ParcialmenteRechazada;
            else
                return EnumEstadoNotaPedido.Pendiente;
        }
    }
}