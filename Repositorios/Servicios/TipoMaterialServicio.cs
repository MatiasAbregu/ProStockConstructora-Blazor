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
    public class TipoMaterialServicio : ITipoMaterialServicio
    {
        private readonly AppDbContext baseDeDatos;

        public TipoMaterialServicio(AppDbContext baseDeDatos)
        {
            this.baseDeDatos = baseDeDatos;
        }

        public async Task<Response<List<TipoMaterialDTO>>> ObtenerTiposDeMaterial(long EmpresaId)
        {
            try
            {
                var tiposMateriales =
                    await baseDeDatos.TipoMateriales.Where(s => s.EmpresaId == EmpresaId)
                    .Select(tm => new TipoMaterialDTO
                    {
                        Nombre = tm.Nombre,
                        Id = tm.Id,
                    }).ToListAsync();

                if (tiposMateriales.Count == 0)
                {
                    return new Response<List<TipoMaterialDTO>>()
                    {
                        Objeto = null,
                        Mensaje = "No hay tipos de materiales registrados.",
                        Estado = true
                    };
                }

                return new Response<List<TipoMaterialDTO>>()
                {
                    Objeto = tiposMateriales,
                    Mensaje = "¡Tipos de materiales cargados con éxito!",
                    Estado = true
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new Response<List<TipoMaterialDTO>>()
                {
                    Estado = false,
                    Mensaje = "¡Hubo un error al cargar los tipos de materiales!",
                    Objeto = null
                };
            }
        }

        public async Task<Response<TipoMaterialDTO>> ObtenerTipoMaterialPorId(long Id)
        {
            try
            {
                var tipoMaterial = await baseDeDatos.TipoMateriales.FirstOrDefaultAsync(tm => tm.Id == Id);
                if (tipoMaterial == null)
                {
                    Response<TipoMaterialDTO>
                    res = new Response<TipoMaterialDTO>()
                    { Estado = true };
                    return res;
                }
                var tipoMaterialDTO = new TipoMaterialDTO
                {
                    Nombre = tipoMaterial.Nombre,
                    Id = tipoMaterial.Id,
                };
                Response<TipoMaterialDTO> response = new Response<TipoMaterialDTO>()
                {
                    Objeto = tipoMaterialDTO,
                    Mensaje = "Tipo de material obtenido exitosamente.",
                    Estado = true
                };
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException.Message}");
                Response<TipoMaterialDTO> response = new Response<TipoMaterialDTO>() { Estado = false };
                return response;
            }
        }


        public async Task<Response<string>> TipoMaterialCargar(TipoMaterialDTO tipoMaterial, long empresaId)
        {
            try
            {
                var ExisteTipoMaterial = await baseDeDatos.TipoMateriales.Where(tm => tm.Nombre == tipoMaterial.Nombre && tm.EmpresaId == empresaId).ToListAsync();
                if (ExisteTipoMaterial != null && ExisteTipoMaterial.Count > 0)
                {
                    return new Response<string>
                    {
                        Objeto = null,
                        Mensaje = "Ya existe un tipo de material con ese nombre.",
                        Estado = false
                    };
                }

                if (tipoMaterial == null || string.IsNullOrWhiteSpace(tipoMaterial.Nombre))
                {
                    return new Response<string>
                    {
                        Objeto = null,
                        Mensaje = "Datos de tipo de material inválidos.",
                        Estado = false
                    };
                }

                var nuevoTipoMaterial = new TipoMaterial
                {
                    Nombre = tipoMaterial.Nombre,
                    EmpresaId = empresaId
                };
                await baseDeDatos.TipoMateriales.AddAsync(nuevoTipoMaterial);
                await baseDeDatos.SaveChangesAsync();
                return new Response<string>
                {
                    Objeto = null,
                    Mensaje = "Tipo de material cargado exitosamente.",
                    Estado = true
                };
            }
            catch (Exception ex)
            {
                return new Response<string>
                {
                    Objeto = null,
                    Mensaje = $"Hubo un error interno en el servidor",
                    Estado = false
                };
            }
        }

        public async Task<Response<string>> TipoMaterialModificar(TipoMaterialDTO tipoMaterialDTO, long Id)
        {
            try
            {
                if (tipoMaterialDTO == null || string.IsNullOrWhiteSpace(tipoMaterialDTO.Nombre))
                {
                    return new Response<string>
                    {
                        Objeto = null,
                        Mensaje = "Datos de tipo de material inválidos.",
                        Estado = false
                    };
                }

                string nuevoNombre = tipoMaterialDTO.Nombre.Trim();

                var tipoMaterial = await baseDeDatos.TipoMateriales.FirstOrDefaultAsync(tm => tm.Id == Id);

                if (tipoMaterial == null)
                {
                    return new Response<string>
                    {
                        Objeto = null,
                        Mensaje = "El tipo de material no existe.",
                        Estado = false
                    };
                }

                bool nombreExistente = await baseDeDatos.TipoMateriales
                    .AnyAsync(tm => tm.Nombre == nuevoNombre && tm.Id != Id);

                if (nombreExistente)
                {
                    return new Response<string>
                    {
                        Objeto = null,
                        Mensaje = "El nombre del tipo de material ya está en uso.",
                        Estado = false
                    };
                }
                tipoMaterial.Nombre = nuevoNombre;

                baseDeDatos.TipoMateriales.Update(tipoMaterial);
                await baseDeDatos.SaveChangesAsync();

                return new Response<string>
                {
                    Objeto = null,
                    Mensaje = "Tipo de material modificado exitosamente.",
                    Estado = true
                };
            }
            catch (Exception ex)
            {
                return new Response<string>
                {
                    Objeto = null,
                    Mensaje = "Error al modificar el tipo de material",
                    Estado = false
                };
            }
        }
    }
}
