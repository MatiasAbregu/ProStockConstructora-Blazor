using BD;
using BD.Modelos;
using DTO.DTOs_Remitos;
using DTO.DTOs_Response;
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
        public async Task<Response<ActualizarRemitoDTO>> ObtenerRemitoPorId(long id)
        {
            try
            {
                var remito = await baseDeDatos.Remitos.FirstOrDefaultAsync(r => r.Id == id);
                if (remito == null)
                {
                    return new Response<ActualizarRemitoDTO>
                    {
                        Objeto = null,
                        Mensaje = "No se encontró el remito.",
                        Estado = false
                    };
                }
                var remitoDTO = new ActualizarRemitoDTO
                {
                    Id = remito.Id,
                    NumeroRemito = remito.NumeroRemito,
                    NotaDePedidoId = remito.NotaDePedidoId,
                    DepositoOrigenId = remito.DepositoOrigenId,
                    DepositoDestinoId = remito.DepositoDestinoId,
                    EstadoRemito = remito.EstadoRemito,
                    NombreTransportista = remito.NombreTransportista,
                    FechaEmision = remito.FechaEmision,
                    FechaLimite = remito.FechaLimite,
                    FechaRecepcion = remito.FechaRecepcion
                };
                return new Response<ActualizarRemitoDTO>
                {
                    Objeto = remitoDTO,
                    Mensaje = "Remito obtenido con éxito.",
                    Estado = true
                };

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException.Message}");
                return new Response<ActualizarRemitoDTO>
                {
                    Objeto = null,
                    Mensaje = "Error al obtener el remito.",
                    Estado = false
                };
            }
        }
        public async Task<Response<ActualizarRemitoDTO>> ObtenerRemitoPorNotaDePedidoId(long notaDePedidoId)
            {
            try
            {
                var remito = await baseDeDatos.Remitos
                    .FirstOrDefaultAsync(r => r.NotaDePedidoId == notaDePedidoId);
                if (remito == null)
                {
                    return new Response<ActualizarRemitoDTO>
                    {
                        Objeto = null,
                        Mensaje = "No se encontró el remito para la nota de pedido dada.",
                        Estado = false
                    };
                }
                var remitoDTO = new ActualizarRemitoDTO
                {
                    Id = remito.Id,
                    NumeroRemito = remito.NumeroRemito,
                    NotaDePedidoId = remito.NotaDePedidoId,
                    DepositoOrigenId = remito.DepositoOrigenId,
                    DepositoDestinoId = remito.DepositoDestinoId,
                    EstadoRemito = remito.EstadoRemito,
                    NombreTransportista = remito.NombreTransportista,
                    FechaEmision = remito.FechaEmision,
                    FechaLimite = remito.FechaLimite,
                    FechaRecepcion = remito.FechaRecepcion
                };
                return new Response<ActualizarRemitoDTO>
                {
                    Objeto = remitoDTO,
                    Mensaje = "Remito obtenido con éxito.",
                    Estado = true
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException.Message}");
                return new Response<ActualizarRemitoDTO>
                {
                    Objeto = null,
                    Mensaje = "Error al obtener el remito.",
                    Estado = false
                };
            }
        }

        public async Task<Response<string>> CrearRemito(CrearRemitoDTO remitoDTO)
        {
            try
            {
                bool existe = await baseDeDatos.Remitos
                    .AnyAsync(r => r.NumeroRemito == remitoDTO.NumeroRemito);
                if (existe) return new Response<string>
                {

                    Objeto = null,  
                    Mensaje = "Ya existe un remito con ese número.",
                    Estado = false

                };
                var nuevoRemito = new Remito
                {
                    NumeroRemito = remitoDTO.NumeroRemito,
                    NotaDePedidoId = remitoDTO.NotaDePedidoId,
                    DepositoOrigenId = remitoDTO.DepositoOrigenId,
                    DepositoDestinoId = remitoDTO.DepositoDestinoId,
                    EstadoRemito = remitoDTO.EstadoRemito,
                    NombreTransportista = remitoDTO.NombreTransportista,
                    FechaEmision = remitoDTO.FechaEmision,
                    FechaLimite = remitoDTO.FechaLimite,
                    FechaRecepcion = remitoDTO.FechaRecepcion
                };
                baseDeDatos.Remitos.Add(nuevoRemito);
                await baseDeDatos.SaveChangesAsync();
                return new Response<string>
                {
                    Objeto = null,
                    Mensaje = "Remito creado con éxito.",
                    Estado = true
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new Response<string>
                {
                    Objeto = null,
                    Mensaje = "Error al crear el remito.",
                    Estado = false
                };
            }
        }
        public async Task<Response<string>> ActualizarRemito(long id, ActualizarRemitoDTO remitoDTO)
        {
            try
            {
                var remitoExistente = await baseDeDatos.Remitos.FirstOrDefaultAsync(r => r.Id == id);
                if (remitoExistente == null)
                {
                    return new Response<string>
                    {
                        Objeto = null,
                        Mensaje = "No existe un remito con ese ID.",
                        Estado = false
                    };
                }
                remitoExistente.NumeroRemito = remitoDTO.NumeroRemito;
                remitoExistente.NotaDePedidoId = (long)remitoDTO.NotaDePedidoId;
                remitoExistente.DepositoOrigenId = (long)remitoDTO.DepositoOrigenId;
                remitoExistente.DepositoDestinoId = (long)remitoDTO.DepositoDestinoId;
                remitoExistente.EstadoRemito = remitoDTO.EstadoRemito;
                remitoExistente.NombreTransportista = remitoDTO.NombreTransportista;
                remitoExistente.FechaEmision = (DateTime)remitoDTO.FechaEmision;
                remitoExistente.FechaLimite = (DateTime)remitoDTO.FechaLimite;
                remitoExistente.FechaRecepcion = remitoDTO.FechaRecepcion;
                await baseDeDatos.SaveChangesAsync();
                return new Response<string>
                {
                    Objeto = null,
                    Mensaje = "Remito actualizado con éxito.",
                    Estado = true
                };
            }
            catch (Exception ex)
            {
                return new Response<string>
                {
                    Objeto = null,
                    Mensaje = "Error al actualizar el remito.",
                    Estado = false
                };
            }
        }
    }

}
