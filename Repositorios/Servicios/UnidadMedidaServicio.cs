using BD;
using BD.Modelos;
using DTO.DTOs_Recursos;
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
    public class UnidadMedidaServicio : IUnidadMedidaServicio
    {
        private readonly AppDbContext baseDeDatos;

        public UnidadMedidaServicio(AppDbContext baseDeDatos)
        {
            this.baseDeDatos = baseDeDatos;
        }

        public async Task<Response<string>>UnidadDeMedidaCargar(UnidadDeMedidaDTO unidadDeMedidaDTO)
        {
           try
           {   
             var existeUnidad = await baseDeDatos.UnidadMedidas
                    .FirstOrDefaultAsync(um => um.Nombre.ToUpper() == unidadDeMedidaDTO.Nombre.ToUpper()
                                        && um.EmpresaId == unidadDeMedidaDTO.EmpresaId);

             if (existeUnidad == null)
             { 
                var nuevaUnidad = new UnidadMedida
                {
                    Nombre = unidadDeMedidaDTO.Nombre,
                    Simbolo = unidadDeMedidaDTO.Simbolo,
                    EmpresaId = unidadDeMedidaDTO.EmpresaId
                };

                baseDeDatos.UnidadMedidas.Add(nuevaUnidad);
                await baseDeDatos.SaveChangesAsync();

                return new Response<string>
                {
                    Objeto = null,
                    Mensaje = "Unidad de medida creada con éxito.",
                    Estado = true
                };
             }
             else
             {
                return new Response<string>
                {
                    Objeto = null,
                    Mensaje = "La unidad de medida ya existe.",
                    Estado = true
                };
             }
           }
           catch(Exception ex)
           {
              Console.WriteLine(ex.Message);
              Response<string> response = new Response<string>() { Estado = false, Mensaje = "Error al cargar la unidad de medida." };
              return response;
           }
        }

        public async Task<Response<List<UnidadDeMedidaDTO>>>ObtenerUnidadesDeMedida(long EmpresaId)
        {
            try
            {
                var existeUnidades = await baseDeDatos.UnidadMedidas
                                    .Where(um => um.EmpresaId == EmpresaId).ToListAsync();

                if (existeUnidades != null && existeUnidades.Count > 0)
                {
                    var unidadesDTO = existeUnidades.Select(um => new UnidadDeMedidaDTO
                    {
                        Id = um.Id,
                        Nombre = um.Nombre,
                        Simbolo = um.Simbolo,
                        EmpresaId = um.EmpresaId
                    }).ToList();
                    return new Response<List<UnidadDeMedidaDTO>>
                    {
                        Objeto = unidadesDTO,
                        Mensaje = "Unidades de medida obtenidas con éxito.",
                        Estado = true
                    };
                }
                else
                {
                    return new Response<List<UnidadDeMedidaDTO>>
                    {
                        Objeto = null,
                        Mensaje = "No hay unidades de medida registradas.",
                        Estado = true
                    };                 
                }
            }
            catch(Exception ex)
            { 
                Console.WriteLine(ex.Message);
                Response<List<UnidadDeMedidaDTO>> response = new Response<List<UnidadDeMedidaDTO>>() { Estado = false, Mensaje = "Error al obtener las unidades de medida." };
                return response;
            }
        }

        public async Task<Response<UnidadDeMedidaDTO>> ObtenerUnidadDeMedidaPorId(long id)
        {
            try
            {
                var um = await baseDeDatos.UnidadMedidas.FindAsync(id);
                if (um == null) return new Response<UnidadDeMedidaDTO>()
                {
                    Estado = true,
                    Mensaje = "No existe una unidad de medida con ese ID",
                    Objeto = null
                };

                return new Response<UnidadDeMedidaDTO>()
                {
                    Estado = true,
                    Mensaje = null,
                    Objeto = new UnidadDeMedidaDTO()
                    {
                        Id = um.Id,
                        Nombre = um.Nombre,
                        Simbolo = um.Simbolo,
                        EmpresaId = um.EmpresaId,
                    }
                };
            } catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return new Response<UnidadDeMedidaDTO>()
                {
                    Estado = false,
                    Mensaje = "¡Hubo un error en el servidor al cargar la unidad de medida!",
                    Objeto = null
                };
            }
        }

        public async Task<Response<string>>UnidadDeMedidaModificar(UnidadDeMedidaDTO unidadDeMedidaDTO, long id)
        {
            try
            {
                var unidadExistente = await baseDeDatos.UnidadMedidas.FindAsync(id);
                var nombreDisponible = await baseDeDatos.UnidadMedidas
                    .AnyAsync(um => um.Nombre.ToUpper() == unidadDeMedidaDTO.Nombre.ToUpper()
                                && um.Id != id && um.EmpresaId == unidadDeMedidaDTO.EmpresaId);

                if (nombreDisponible) return new Response<string>()
                {
                    Estado = true,
                    Mensaje = "El nombre de la unidad de medida ya está en uso.",
                    Objeto = null
                };

                if (unidadExistente != null)
                {
                    unidadExistente.Nombre = unidadDeMedidaDTO.Nombre;
                    unidadExistente.Simbolo = unidadDeMedidaDTO.Simbolo;
                    await baseDeDatos.SaveChangesAsync();

                    return new Response<string>
                    {
                        Objeto = null,
                        Mensaje = "Unidad de medida modificada con éxito.",
                        Estado = true
                    };
                }
                else
                {
                    return new Response<string>
                    {
                        Objeto = null,
                        Mensaje = "La unidad de medida no existe.",
                        Estado = true
                    };
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Response<string> response = new() 
                { Estado = false, Mensaje = "Error al modificar la unidad de medida." };
                return response;
            }
        }
    }
}
