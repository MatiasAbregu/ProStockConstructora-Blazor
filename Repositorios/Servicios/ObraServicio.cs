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

        public async Task<Response<List<VerObraDTO>>> ObtenerObrasPorUsuario(DatosUsuario usuario)
        {
            try
            {
                if (usuario == null) return new Response<List<VerObraDTO>>()
                { Objeto = null, Estado = true, Mensaje = "No hay un usuario logueado." };

                if (usuario.Roles.Contains("ADMINISTRADOR"))
                {
                    var obras = await baseDeDatos.Obras.
                       Where(o => o.EmpresaId == usuario.EmpresaId && o.Estado != EnumEstadoObra.Finalizada)
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
                       Where(o => usuario.ObrasId.Contains(o.Id) && o.Estado != EnumEstadoObra.Finalizada)
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

        public async Task<Response<ObraActualizarDTO>> ObtenerObraPorId(long obraId)
        {
            try
            {
                Obra o = await baseDeDatos.Obras.FirstOrDefaultAsync(o => o.Id == obraId);
                if (o == null)
                    return new Response<ObraActualizarDTO>
                    {
                        Estado = true,
                        Objeto = null,
                        Mensaje = "No existe la obra con el ID proporcionado."
                    };

                ObraActualizarDTO obra = new()
                {
                    Id = o.Id,
                    CodigoObra = o.CodigoObra,
                    NombreObra = o.NombreObra,
                    Estado = (DTO.Enum.EnumEstadoObra)o.Estado
                };
                return new Response<ObraActualizarDTO>
                {
                    Estado = true,
                    Objeto = obra,
                    Mensaje = "Obra obtenida con éxito."
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException.Message}");
                return new Response<ObraActualizarDTO>
                {
                    Estado = false,
                    Objeto = null,
                    Mensaje = "¡Hubo un error desde el servidor al cargar la obra!"
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

        public async Task<Response<string>> CrearObra(CrearObraDTO obraDTO)
        {
            try
            {
                bool existeObra = await baseDeDatos.Obras
                    .AnyAsync(ob => obraDTO.CodigoObra.ToLower() == ob.CodigoObra.ToLower()
                    && ob.EmpresaId == obraDTO.EmpresaId);

                if (existeObra)
                    return new Response<string>()
                    {
                        Estado = true,
                        Mensaje = "Ya existe una obra con ese código en la empresa.",
                        Objeto = null
                    };

                var nuevaObra = new Obra
                {
                    CodigoObra = obraDTO.CodigoObra,
                    NombreObra = obraDTO.NombreObra,
                    EmpresaId = obraDTO.EmpresaId,
                    Estado = (EnumEstadoObra)obraDTO.Estado
                };
                baseDeDatos.Obras.Add(nuevaObra);
                await baseDeDatos.SaveChangesAsync();
                return new Response<string>()
                {
                    Estado = true,
                    Mensaje = null,
                    Objeto = "¡Obra creada con éxito!"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException.Message}");
                return new Response<string>()
                {
                    Estado = false,
                    Objeto = null,
                    Mensaje = "¡Hubo un error desde el servidor al crear la obra!"
                };
            }
        }

        public async Task<Response<string>> ActualizarObra(long id, ObraActualizarDTO o)
        {
            try
            {
                Obra obraUpdate = await baseDeDatos.Obras.FirstOrDefaultAsync(ob => ob.Id == id);
                if (obraUpdate == null) return new Response<string>()
                {
                    Estado = true,
                    Mensaje = "No existe una obra con ese ID.",
                    Objeto = null
                };

                bool existeCodigo = await baseDeDatos.Obras
                            .AnyAsync(ob => ob.CodigoObra == o.CodigoObra && o.Id != ob.Id);
                if (existeCodigo) return new Response<string>()
                {
                    Estado = true,
                    Mensaje = "Ya existe una obra con ese código.",
                    Objeto = null
                };

                obraUpdate.CodigoObra = o.CodigoObra;
                obraUpdate.NombreObra = o.NombreObra;
                obraUpdate.Estado = (EnumEstadoObra)o.Estado;

                await baseDeDatos.SaveChangesAsync();
                return new Response<string>()
                {
                    Estado = true,
                    Objeto = "Obra actualizada con éxito.",
                    Mensaje = null
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException.Message}");
                return new Response<string>()
                {
                    Estado = false,
                    Objeto = null,
                    Mensaje = "¡Hubo un error desde el servidor al actualizar la obra!"
                };
            }
        }

        public async Task<Response<string>> FinalizarObra(long id)
        {
            try
            {
                var obra = await baseDeDatos.Obras.FirstOrDefaultAsync(o => o.Id == id);
                if (obra == null)
                    return new Response<string>()
                    {
                        Estado = true,
                        Objeto = null,
                        Mensaje = "No existe una obra con ese ID."
                    };

                obra.Estado = EnumEstadoObra.Finalizada;

                await baseDeDatos.SaveChangesAsync();
                return new Response<string>()
                {
                    Estado = true,
                    Objeto = "Obra finalizada con éxito.",
                    Mensaje = null
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException.Message}");
                return new Response<string>()
                {
                    Estado = false,
                    Objeto = null,
                    Mensaje = "¡Hubo un error desde el servidor al finalizar la obra!"
                };
            }
        }
    }
}