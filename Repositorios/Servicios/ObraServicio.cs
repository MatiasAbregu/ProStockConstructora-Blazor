using BD;
using BD.Enums;
using BD.Modelos;
using DTO.DTOs_Obras;
using Repositorios.Implementaciones;
using Microsoft.EntityFrameworkCore;
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

        public async Task<(bool, List<VerObraDTO>)> ObtenerObras(int EmpresaId)
        {
            try
            {
                List<VerObraDTO> obrasVer = [];
                List<Obra> obras = await baseDeDatos.Obras.
                    Where(o => o.EmpresaId == EmpresaId).ToListAsync();

                foreach (Obra o in obras)
                {
                    obrasVer.Add(new VerObraDTO
                    {
                        Id = o.Id,
                        CodigoObra = o.CodigoObra,
                        NombreObra = o.NombreObra,                       
                        Estado = o.Estado.ToString() == "EnProceso" ? "En proceso" : o.Estado.ToString()
                    });
                }

                return (true, obrasVer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException.Message}");
                return (false, null);
            }
        }

        public async Task<(bool, VerObraDTO)> ObtenerObraPorId(int id)
        {
            try
            {
                Obra o = await baseDeDatos.Obras.FirstOrDefaultAsync(o => o.Id == id);
                if (o == null) return (true, null);
                VerObraDTO obraVer = new VerObraDTO
                {
                    Id = o.Id,
                    CodigoObra = o.CodigoObra,
                    NombreObra = o.NombreObra,
                    Estado = o.Estado.ToString() == "EnProceso" ? "En proceso" : o.Estado.ToString()
                };
                return (true, obraVer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException.Message}");
                return (false, null);
            }
        }

        public async Task<(bool, string)> CrearObra(CrearObraDTO obraDTO)
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

        public async Task<(bool, string)> ActualizarObra(int id, ObraActualizarDTO o)
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