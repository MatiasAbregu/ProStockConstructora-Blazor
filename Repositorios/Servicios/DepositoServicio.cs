using BD;
using BD.Modelos;
using DTO.DTOs_Depositos;
using DTO.DTOs_Ubicacion;
using Repositorios.Implementaciones;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.DTOs_Response;

namespace Repositorios.Servicios
{
    public class DepositoServicio : IDepositoServicio
    {
        private readonly AppDbContext baseDeDatos;

        public DepositoServicio(AppDbContext BaseDeDatos)
        {
            this.baseDeDatos = BaseDeDatos;

        }
        public async Task<Response<List<VerDepositoDTO>>> ObtenerDepositos()
        {
            var response = new Response<List<VerDepositoDTO>>();
            try
            {
                var depositosDeLaDB = await baseDeDatos.Depositos
                                                 .Include(o => o.Ubicacion)
                                                 .ToListAsync();

                var depositosDTO = depositosDeLaDB.Select(deposito => new VerDepositoDTO
                {
                    Id = deposito.Id,
                    CodigoDeposito = deposito.CodigoDeposito,
                    NombreDeposito = deposito.NombreDeposito,
                    TipoDeposito = deposito.TipoDeposito.ToString() == "EnUso" ? "En uso" : deposito.TipoDeposito.ToString(),
                    Ubicacion = new UbicacionDTO
                    {
                        Id = deposito.Ubicacion.Id,
                        CodigoUbicacion = deposito.Ubicacion.CodigoUbicacion,
                        UbicacionDomicilio = deposito.Ubicacion.Domicilio
                    }
                }).ToList();
                return new Response<List<VerDepositoDTO>>
                {
                    Objeto = depositosDTO,
                    Mensaje = "Depósito encontrado.",
                    Estado = true
                };
            }
            catch (Exception ex)
            {
                return new Response<List<VerDepositoDTO>>
                {
                    Objeto = null,
                    Mensaje = "Error al obtener el depósito.",
                    Estado = false
                };
            }


        }
        public async Task<Response<List<VerDepositoDTO>>>ObtenerDepositoPorId(int id)
        {
            try
            {
                var deposito = await baseDeDatos.Depositos
                    .Include(o => o.Ubicacion).ThenInclude(u => u.Provincia)
                    .FirstOrDefaultAsync(d => d.Id == id);
                if (deposito != null)
                {
                    var depositoDTO = new VerDepositoDTO
                    {
                        Id = deposito.Id,
                        CodigoDeposito = deposito.CodigoDeposito,
                        NombreDeposito = deposito.NombreDeposito,
                        TipoDeposito = deposito.TipoDeposito.ToString() 
                        == "EnUso" ? "En uso" : deposito.TipoDeposito.ToString(),
                        Ubicacion = new UbicacionDTO()
                        {
                            Id = deposito.Ubicacion.Id,
                            CodigoUbicacion = deposito.Ubicacion.CodigoUbicacion,
                            UbicacionDomicilio = deposito.Ubicacion.Domicilio,
                            Provincia = new ProvinciaDTO()
                            {
                                Id = deposito.Ubicacion.Provincia.Id,
                                NombreProvincia = deposito.Ubicacion.Provincia.Nombre
                            }
                        }
                    };
                    return new Response<List<VerDepositoDTO>>
                    {
                        Objeto = new List<VerDepositoDTO> { depositoDTO },
                        Mensaje = "Depósito encontrado.",
                        Estado = true
                    };
                }
                else
                {
                    return new Response<List<VerDepositoDTO>>
                    {
                        Objeto = null,
                        Mensaje = "No existe un depósito con ese ID.",
                        Estado = true
                    };
                }
            }
            catch (Exception ex)
            {
                return new Response<List<VerDepositoDTO>>
                {
                    Objeto = null,
                    Mensaje = "Error al obtener el depósito.",
                    Estado = false
                };
            }
        }

        public async Task<Response<List<VerDepositoDTO>>> ObtenerDepositosPorObraId(int obraId)
        {
            try
            {
                var depositos = await baseDeDatos.Depositos.Where(o => o.ObraId == obraId)
                    .Include(o => o.Ubicacion).ThenInclude(u => u.Provincia).ToListAsync();

                if (depositos != null && depositos.Count > 0)
                {
                
                    return new Response<List<VerDepositoDTO>>
                    {
                        Objeto = depositos.Select(deposito => new VerDepositoDTO
                        {
                            Id = deposito.Id,
                            CodigoDeposito = deposito.CodigoDeposito,
                            NombreDeposito = deposito.NombreDeposito,
                            TipoDeposito = deposito.TipoDeposito.ToString() 
                            == "EnUso" ? "En uso" : deposito.TipoDeposito.ToString(),
                            Ubicacion = new UbicacionDTO()
                            {
                                Id = deposito.Ubicacion.Id,
                                CodigoUbicacion = deposito.Ubicacion.CodigoUbicacion,
                                UbicacionDomicilio = deposito.Ubicacion.Domicilio,
                                Provincia = new ProvinciaDTO()
                                {
                                    Id = deposito.Ubicacion.Provincia.Id,
                                    NombreProvincia = deposito.Ubicacion.Provincia.Nombre
                                }
                            }
                        }).ToList(),
                        Mensaje = "Depósitos obtenidos exitosamente.",
                        Estado = true
                    };
                }
                else
                {
                    return new Response<List<VerDepositoDTO>>
                    {
                        Objeto = null,
                        Mensaje = "No existen depósitos para esta obra.",
                        Estado = true
                    };
                }
            }
            catch (Exception ex) 
            { 
                return new Response<List<VerDepositoDTO>>
                {
                    Objeto = null,
                    Mensaje = "Error al obtener los depósitos.",
                    Estado = false
                };
            }
            
        }

        public async Task<Response<int>>CrearDeposito(DepositoAsociarDTO e)
        {
            try
            {
                var nuevoDeposito = new Deposito
                {
                    CodigoDeposito = e.CodigoDeposito,
                    NombreDeposito = e.NombreDeposito,
                    ObraId = e.ObraId,
                    TipoDeposito = (BD.Enums.EnumTipoDeposito)e.TipoDeposito,
                    Ubicacion = await BuscarUbicacion(e.Ubicacion),
                    UbicacionId = e.Ubicacion.Id
                };
                await baseDeDatos.Depositos.AddAsync(nuevoDeposito);
                await baseDeDatos.SaveChangesAsync();

               return new Response<int>
                {
                    Objeto = (int)nuevoDeposito.Id,
                    Mensaje = "Depósito creado exitosamente.",
                    Estado = true
                };
            }
            catch (Exception )
            {

                return new Response<int>
                {
                    Objeto = 0,
                    Mensaje = "Error al obtener los depósitos.",
                    Estado = false
                };
            }
        }

        public async Task<Response<string>> ActualizarDeposito(int id,DepositoAsociarDTO e)
        {
            try
            {
                Deposito? deposito = await baseDeDatos.Depositos
                    .FirstOrDefaultAsync(d => d.Id == e.Id);
                if (deposito == null)
                {
                    return new Response<string>
                    {
                        Objeto = null,
                        Mensaje = "El depósito no existe.",
                        Estado = false
                    };
                }

                bool existeCodigo = await baseDeDatos.Depositos
                            .AnyAsync(d => d.CodigoDeposito == e.CodigoDeposito && d.Id != e.Id);

                if (existeCodigo)
                {
                    return new Response<string>
                    {
                        Objeto = null,
                        Mensaje = "Ese código ya está en uso",
                        Estado = false
                    };
                }

                deposito.CodigoDeposito = e.CodigoDeposito;
                deposito.NombreDeposito = e.NombreDeposito;
                deposito.ObraId = e.ObraId;
                deposito.TipoDeposito = (BD.Enums.EnumTipoDeposito)e.TipoDeposito;
                deposito.Ubicacion = await BuscarUbicacion(e.Ubicacion);

                baseDeDatos.Depositos.Update(deposito);
                await baseDeDatos.SaveChangesAsync();
                return new Response<string>
                {
                    Objeto = deposito.Id.ToString(),
                    Mensaje = "Depósito actualizado exitosamente.",
                    Estado = true
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new Response<string>
                {
                    Objeto = null,
                    Mensaje = "Error al actualizar el depósito.",
                    Estado = false
                };
            }
        }

        public async Task<Response<string>> EliminarDeposito(long id)
        {
            try
            {
                var deposito = await baseDeDatos.Depositos.FirstOrDefaultAsync(d => d.Id == id);
                if (deposito == null)
                {
                    return new Response<string>
                    {
                        Objeto = null,
                        Mensaje = "No existe un depósito con ese ID.",
                        Estado = false
                    };
                }

                baseDeDatos.Depositos.Remove(deposito);
                await baseDeDatos.SaveChangesAsync();

                return new Response<string>
                {
                    Objeto = deposito.Id.ToString(),
                    Mensaje = "Depósito eliminado exitosamente.",
                    Estado = true
                };
            }
            catch (Exception)
            {
                return new Response<string>
                {
                    Objeto = null,
                    Mensaje = "Error al eliminar el depósito.",
                    Estado = false
                };
            }
        }

        public async Task<Ubicacion> BuscarUbicacion(UbicacionDTO ubicacion)
        {
            Ubicacion? resUbicacion = null;
            Provincia? resProvincia = null;
            if (ubicacion.Id == 0)
            {
                resUbicacion = baseDeDatos.Ubicaciones
                    .FirstOrDefault(u => u.CodigoUbicacion.ToUpper() == ubicacion.CodigoUbicacion.ToUpper());

                if (resUbicacion == null)
                {
                    if (ubicacion.Provincia.Id == 0)
                    {
                        resProvincia = baseDeDatos.Provincias
                        .FirstOrDefault(p => p.Nombre == ubicacion.Provincia.NombreProvincia.ToUpper());

                        if (resProvincia == null)
                        {
                            resProvincia = new Provincia()
                            { Nombre = ubicacion.Provincia.NombreProvincia.ToUpper() };
                            baseDeDatos.Provincias.Add(resProvincia);
                            await baseDeDatos.SaveChangesAsync();
                        }
                    }

                    resUbicacion = new Ubicacion()
                    {
                        CodigoUbicacion = ubicacion.CodigoUbicacion.ToUpper(),
                        Domicilio = ubicacion.UbicacionDomicilio.ToUpper(),
                        ProvinciaId = ubicacion.Provincia.Id != 0 ? ubicacion.Provincia.Id : resProvincia!.Id
                    };
                    baseDeDatos.Ubicaciones.Add(resUbicacion);
                    await baseDeDatos.SaveChangesAsync();
                }
            }

            return resUbicacion;
        }
    }
}