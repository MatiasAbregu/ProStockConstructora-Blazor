using BD;
using BD.Enums;
using BD.Modelos;
using DTO.DTOs_Obras;
using DTO.DTOs_Response;
using DTO.DTOs_Usuarios;
using Microsoft.EntityFrameworkCore;
using Repositorios.Implementaciones;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.Servicios
{
    public class ObraServicio : IObraServicio
    {
        private readonly AppDbContext baseDeDatos;

        public ObraServicio(AppDbContext baseDeDatos)
        {
            this.baseDeDatos = baseDeDatos;
        }
        public async Task<Response<List<ObraEmpresaDTO>>> ObtenerObrasDeEmpresa(long EmpresaId)
        {
            try
            {
                List<Obra> obras = await baseDeDatos.Obras.
                    Where(o => o.EmpresaId == EmpresaId).ToListAsync();

                if (obras.Count == 0)
                {
                    return new Response<List<ObraEmpresaDTO>>()
                    { Objeto = [], Mensaje = "Aún no existen obras para esta empresa.", Estado = true };
                }

                return new Response<List<ObraEmpresaDTO>>()
                {
                    Objeto = obras.Select(o => new ObraEmpresaDTO()
                    {
                        Id = o.Id,
                        CodigoObra = o.CodigoObra,
                        NombreObra = o.NombreObra
                    }).ToList(),
                    Estado = true,
                    Mensaje = "¡Obras cargadas con éxito!"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException.Message}");
                return new Response<List<ObraEmpresaDTO>>()
                {
                    Objeto = null,
                    Estado = false,
                    Mensaje = $"¡Hubo un error desde el servidor al cargar las obras!"
                };
            }
        }

        public async Task<Response<List<VerObraDTO>>> ObtenerObrasPorUsuario(DatosUsuario usuario)
        {
            try
            {
                if (usuario == null) return new Response<List<VerObraDTO>>()
                { Objeto = null, Estado = true, Mensaje = "No hay un usuario logueado." };

                if (usuario.Roles.Contains("ADMINISTRADOR"))
                {
                    var obras = await baseDeDatos.Obras.
                       Where(o => o.EmpresaId == usuario.EmpresaId)
                       .Select(o => new VerObraDTO()
                       {
                           Id = o.Id,
                           CodigoObra = o.CodigoObra,
                           NombreObra = o.NombreObra,
                           Estado = o.Estado.ToString() == "EnProceso" ? "En proceso" : o.Estado.ToString()
                       }).ToListAsync();

                    return new Response<List<VerObraDTO>>()
                    { Objeto = obras, Estado = true, Mensaje = "¡Obras cargadas con éxito!" };
                }
                else if (usuario.Roles.Contains("JEFEDEOBRA"))
                {
                    var obras = await baseDeDatos.Obras.
                       Where(o => usuario.ObrasId.Contains(o.Id))
                       .Select(o => new VerObraDTO()
                       {
                           Id = o.Id,
                           CodigoObra = o.CodigoObra,
                           NombreObra = o.NombreObra,
                           Estado = o.Estado.ToString() == "EnProceso" ? "En proceso" : o.Estado.ToString()
                       }).ToListAsync();

                    return new Response<List<VerObraDTO>>()
                    { Objeto = obras, Estado = true, Mensaje = "¡Obras cargadas con éxito!" };
                }
                else
                {
                    return new Response<List<VerObraDTO>>()
                    { Objeto = null, Estado = true, Mensaje = "Acceso denegado." };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new Response<List<VerObraDTO>>()
                { Objeto = null, Estado = false, Mensaje = "¡Hubo un error desde el servidor al cargar las obras!" };
            }
        }

        public async Task<Response<VerObraDTO>> ObtenerObraPorId(int obraId)
        {
            try
            {
                Obra o = await baseDeDatos.Obras.FirstOrDefaultAsync(o => o.Id == obraId);
                if (o == null)
                    return new Response<VerObraDTO>
                    {
                        Estado = true,
                        Objeto = null,
                        Mensaje = "No existe la obra con el ID proporcionado."
                    };

                VerObraDTO VerObraDTO = new VerObraDTO
                {
                    Id = o.Id,
                    CodigoObra = o.CodigoObra,
                    NombreObra = o.NombreObra,
                    Estado = o.Estado.ToString() == "EnProceso" ? "En proceso" : o.Estado.ToString()
                };
                return new Response<VerObraDTO>
                {
                    Estado = true,
                    Objeto = VerObraDTO,
                    Mensaje = "Obra obtenida con éxito."
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException.Message}");
                return new Response<VerObraDTO>
                {
                    Estado = false,
                    Objeto = null,
                    Mensaje = "Error al obtener la obra por el Id."
                };
            }
        }

        public async Task<Response<VerObraDTO>> ObtenerObrasPorCodigoObra(string codigoObra)
        {
            try
            {
                Obra o = await baseDeDatos.Obras.FirstOrDefaultAsync(o => o.CodigoObra.ToLower() == codigoObra.ToLower());
                if (o == null)
                    return new Response<VerObraDTO>
                    {
                        Estado = true,
                        Objeto = null,
                        Mensaje = "No existe la obra con el código proporcionado."
                    };
                VerObraDTO obraVer = new VerObraDTO
                {
                    Id = o.Id,
                    CodigoObra = o.CodigoObra,
                    NombreObra = o.NombreObra,
                    Estado = o.Estado.ToString() == "EnProceso" ? "En proceso" : o.Estado.ToString()
                };
                return new Response<VerObraDTO>
                {
                    Estado = true,
                    Objeto = obraVer,
                    Mensaje = "Obra obtenida con éxito."
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException.Message}");
                return new Response<VerObraDTO>
                {
                    Estado = false,
                    Objeto = null,
                    Mensaje = "Error al obtener la obra por el codigo."
                };
            }
        }

        public async Task<(bool, string)> CrearObra(CrearObraDTO obraDTO) // habria que pasarle el CUIT de la empresa Y EL RESPONSE
        {
            try
            {
                bool existeObra = await baseDeDatos.Obras
                    .AnyAsync(ob => obraDTO.CodigoObra.ToLower() == ob.CodigoObra.ToLower()
                    && ob.EmpresaId == obraDTO.EmpresaId);
                if (existeObra)
                    return (false, "Ya existe una obra con ese código en la empresa.");

                var nuevaObra = new Obra
                {
                    CodigoObra = obraDTO.CodigoObra,
                    NombreObra = obraDTO.NombreObra,
                    EmpresaId = obraDTO.EmpresaId,
                    Estado = (EnumEstadoObra)obraDTO.Estado
                };
                await baseDeDatos.Obras.AddAsync(nuevaObra);
                await baseDeDatos.SaveChangesAsync();
                return (true, "Obra creada con éxito.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException.Message}");
                return (false, "Error al crear la obra.");
            }
        }

        public async Task<(bool, string)> ActualizarObra(int id, ObraActualizarDTO o) // borrarlo o cambiarlo por CODIGO de OBRA
        {
            try
            {
                Obra obraUpdate = await baseDeDatos.Obras.FirstOrDefaultAsync(ob => ob.Id == id);
                if (obraUpdate == null) return (false, "No existe una obra con ese ID.");

                bool existeCodigo = await baseDeDatos.Obras
                            .AnyAsync(ob => ob.CodigoObra == o.CodigoObra && o.Id != ob.Id);
                if (existeCodigo) return (false, "Ya existe una obra con ese código.");

                obraUpdate.CodigoObra = o.CodigoObra;
                obraUpdate.NombreObra = o.NombreObra;
                obraUpdate.Estado = (EnumEstadoObra)o.Estado;

                await baseDeDatos.SaveChangesAsync();
                return (true, "Obra actualizada con éxito.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException.Message}");
                return (false, "Error al actualizar la obra.");
            }
        }
    }
}