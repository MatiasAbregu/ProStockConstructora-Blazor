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

namespace Repositorios.Servicios
{
    public class DepositoServicio : IDepositoServicio
    {
        private readonly AppDbContext baseDeDatos;

        public DepositoServicio(AppDbContext BaseDeDatos)
        {
            this.baseDeDatos = BaseDeDatos;

        }

        public async Task<(bool, VerDepositoDTO)> ObtenerDepositoPorId(int id)
        {
            try
            {
                Deposito? deposito = await baseDeDatos.Depositos
                    .Include(d => d.Ubicacion).ThenInclude(u => u.Provincia)
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (deposito == null) return (true, null);
                VerDepositoDTO depositoVer = new VerDepositoDTO
                {
                    Id = deposito.Id,
                    CodigoDeposito = deposito.CodigoDeposito,
                    NombreDeposito = deposito.NombreDeposito,
                    TipoDeposito = deposito.TipoDeposito.ToString() == "EnUso" ? "En uso" : deposito.TipoDeposito.ToString(),
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
                return (true, depositoVer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return (false, null);
            }
        }

        public async Task<(bool, List<VerDepositoDTO>)> ObtenerDepositosPorObraId(int obraId)
        {
            try
            {
                var depositos = await baseDeDatos.Depositos.Where(o => o.ObraId == obraId)
                    .Include(o => o.Ubicacion).ThenInclude(u => u.Provincia).ToListAsync();

                if (depositos != null && depositos.Count > 0)
                {
                    return (true, depositos.Select(deposito => new VerDepositoDTO
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
                    }).ToList());
                }
                else
                {
                    return (true, null);
                }
            }
            catch (Exception ex) { return (false, null); }
            ;
        }

        public async Task<(bool, string)> CrearDeposito(DepositoAsociarDTO e)
        {
            try
            {
                bool existeDeposito = await baseDeDatos.Depositos
                    .AnyAsync(d => d.CodigoDeposito == e.CodigoDeposito);
                if (existeDeposito) return (false, "Ya existe un depósito con ese código.");

                Ubicacion? resUbicacion = null;
                Provincia? resProvincia = null;
                if (e.Ubicacion.Id == 0)
                {
                    resUbicacion = baseDeDatos.Ubicaciones
                        .FirstOrDefault(u => u.CodigoUbicacion.ToUpper() == e.Ubicacion.CodigoUbicacion.ToUpper());

                    if (resUbicacion == null)
                    {
                        if (e.Ubicacion.Provincia.Id == 0)
                        {
                            resProvincia = baseDeDatos.Provincias
                            .FirstOrDefault(p => p.Nombre == e.Ubicacion.Provincia.NombreProvincia.ToUpper());

                            if (resProvincia == null)
                            {
                                resProvincia = new Provincia()
                                { Nombre = e.Ubicacion.Provincia.NombreProvincia.ToUpper() };
                                baseDeDatos.Provincias.Add(resProvincia);
                                await baseDeDatos.SaveChangesAsync();
                            }
                        }

                        resUbicacion = new Ubicacion()
                        {
                            CodigoUbicacion = e.Ubicacion.CodigoUbicacion.ToUpper(),
                            Domicilio = e.Ubicacion.UbicacionDomicilio.ToUpper(),
                            ProvinciaId = e.Ubicacion.Provincia.Id != 0 ? e.Ubicacion.Provincia.Id : resProvincia!.Id
                        };
                        baseDeDatos.Ubicaciones.Add(resUbicacion);
                        await baseDeDatos.SaveChangesAsync();
                    }
                }

                Deposito nuevoDeposito = new Deposito
                {
                    CodigoDeposito = e.CodigoDeposito,
                    NombreDeposito = e.NombreDeposito,
                    TipoDeposito = (BD.Enums.EnumTipoDeposito)e.TipoDeposito,
                    ObraId = e.ObraId,
                    UbicacionId = e.Ubicacion.Id != 0 ? e.Ubicacion.Id : resUbicacion!.Id
                };

                await baseDeDatos.Depositos.AddAsync(nuevoDeposito);
                await baseDeDatos.SaveChangesAsync();
                return (true, "Depósito creado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return (false, "Error al crear el depósito.");
            }
        }

        public async Task<(bool, string)> ActualizarDeposito(DepositoAsociarDTO e)
        {
            try
            {
                Deposito? deposito = await baseDeDatos.Depositos
                    .FirstOrDefaultAsync(d => d.Id == e.Id);
                if (deposito == null) return (false, "El depósito no existe.");

                bool existeCodigo = await baseDeDatos.Depositos
                            .AnyAsync(d => d.CodigoDeposito == e.CodigoDeposito && d.Id != e.Id);

                if (existeCodigo) return (false, "Ese código ya está en uso");

                deposito.CodigoDeposito = e.CodigoDeposito;
                deposito.NombreDeposito = e.NombreDeposito;
                deposito.ObraId = e.ObraId;
                deposito.TipoDeposito = (BD.Enums.EnumTipoDeposito)e.TipoDeposito;   
                deposito.Ubicacion = await BuscarUbicacion(e.Ubicacion);

                baseDeDatos.Depositos.Update(deposito);
                await baseDeDatos.SaveChangesAsync();
                return (true, "Depósito actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return (false, "Error al actualizar el depósito.");
            }
        }

        public async Task<(bool, string)> EliminarDeposito(int id)
        {
            try
            {
                BD.Modelos.Deposito deposito = await baseDeDatos.Depositos.FirstOrDefaultAsync(d => d.Id == id);
                if (deposito == null) return (false, "No existe un depósito con ese ID.");
                baseDeDatos.Depositos.Remove(deposito);
                await baseDeDatos.SaveChangesAsync();
                return (true, "Depósito eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return (false, "Error al eliminar el depósito.");
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