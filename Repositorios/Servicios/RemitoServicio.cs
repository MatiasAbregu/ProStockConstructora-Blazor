using BD;
using BD.Modelos;
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
        public async Task<Response<List<VerRemitoDTO>>> ObtenerRemitos()
        {
            try
            {
                var remitos = await baseDeDatos.Remitos.ToListAsync();
                var remitosDTO = remitos.Select(r => new VerRemitoDTO
                {
                    Id = r.Id,
                    NumeroRemito = r.NumeroRemito,
                    NotaDePedidoId = r.NotaDePedidoId,
                    DepositoOrigenId = r.DepositoOrigenId,
                    DepositoDestinoId = r.DepositoDestinoId,
                    EstadoRemito = r.EstadoRemito,
                    FechaEmision = r.FechaEmision,
                    FechaRecepcion = r.FechaRecepcion
                }).ToList();
                return new Response<List<VerRemitoDTO>>
                {
                    Objeto = remitosDTO,
                    Mensaje = "Remitos obtenidos con éxito.",
                    Estado = true
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException.Message}");
                return new Response<List<VerRemitoDTO>>
                {
                    Objeto = null,
                    Mensaje = "Error al obtener los remitos.",
                    Estado = false
                };
            }
        }
        public async Task<Response<VerRemitoDTO>> ObtenerRemitoPorId([FromRoute]long id)
        {
            try
            {
                var remito = await baseDeDatos.Remitos
                    .Include(r => r.NotaDePedido)
                    .Include(r => r.DepositoOrigenId)
                    .Include(r => r.DepositoDestinoId)
                    .FirstOrDefaultAsync(r => r.Id == id);
                if (remito == null)
                {
                    return new Response<VerRemitoDTO>
                    {
                        Objeto = null,
                        Mensaje = "No existe el remito con ese ID.",
                        Estado = false
                    };
                }
                var remitoDTO = new VerRemitoDTO()
                {
                    Id = remito.Id,
                    NumeroRemito = remito.NumeroRemito,
                    NotaDePedidoId = remito.NotaDePedidoId,
                    DepositoOrigenId = remito.DepositoOrigenId,
                    DepositoDestinoId = remito.DepositoDestinoId,
                    EstadoRemito = remito.EstadoRemito,
                    FechaEmision = remito.FechaEmision,
                    FechaRecepcion = remito.FechaRecepcion
                };
                return new Response<VerRemitoDTO>
                {
                    Objeto = remitoDTO,
                    Mensaje = "Remito obtenido con éxito.",
                    Estado = true
                };

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException.Message}");
                return new Response<VerRemitoDTO>
                {
                    Objeto = null,
                    Mensaje = "Error al obtener el remito.",
                    Estado = false
                };
            }
        }
        public async Task<Response<VerRemitoDTO>> ObtenerRemitoPorNotaDePedidoId(long notaDePedidoId)
            {
            try
            {
                var remito = await baseDeDatos.Remitos
                    .FirstOrDefaultAsync(r => r.NotaDePedidoId == notaDePedidoId);
                if (remito == null)
                {
                    return new Response<VerRemitoDTO>
                    {
                        Objeto = null,
                        Mensaje = "No se encontró el remito para la nota de pedido dada.",
                        Estado = false
                    };
                }
                var remitoDTO = new VerRemitoDTO
                {
                    Id = remito.Id,
                    NumeroRemito = remito.NumeroRemito,
                    NotaDePedidoId = remito.NotaDePedidoId,
                    DepositoOrigenId = remito.DepositoOrigenId,
                    DepositoDestinoId = remito.DepositoDestinoId,
                    EstadoRemito = remito.EstadoRemito,
                    FechaEmision = remito.FechaEmision,
                    FechaRecepcion = remito.FechaRecepcion
                };
                return new Response<VerRemitoDTO>
                {
                    Objeto = remitoDTO,
                    Mensaje = "Remito obtenido con éxito.",
                    Estado = true
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException.Message}");
                return new Response<VerRemitoDTO>
                {
                    Objeto = null,
                    Mensaje = "Error al obtener el remito.",
                    Estado = false
                };
            }
        }

        public async Task<Response<string>> CrearRemito(CrearRemitoDTO e)
        {
            try
            {
                bool existe = await baseDeDatos.Remitos
                    .AnyAsync(r => r.NumeroRemito == e.NumeroRemito);
                if (existe) return new Response<string>
                {

                    Objeto = null,  
                    Mensaje = "Ya existe un remito con ese número.",
                    Estado = true

                };
                var nuevoRemito = new Remito
                {
                    NumeroRemito = e.NumeroRemito,
                    NotaDePedidoId = e.NotaDePedidoId,
                    DepositoOrigenId = e.DepositoOrigenId,
                    DepositoDestinoId = e.DepositoDestinoId,
                    EstadoRemito = e.EstadoRemito,
                    FechaEmision = e.FechaEmision,
                    FechaRecepcion = e.FechaRecepcion
                };
                baseDeDatos.Remitos.Add(nuevoRemito);
                await baseDeDatos.SaveChangesAsync();

                return new Response<string>
                {
                    Objeto = "Remito creado con éxito.",
                    Mensaje = null,
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
                remitoExistente.FechaEmision = (DateTime)remitoDTO.FechaEmision;
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
