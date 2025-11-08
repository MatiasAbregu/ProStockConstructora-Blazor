using BD;
using BD.Modelos;
using DTO.DTOs_Obras;
using Repositorios.Implementaciones;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.Servicios
{
    public class ObraUsuarioServicio : IObraUsuarioServicio
    {
        private readonly AppDbContext baseDeDatos;
        private readonly IObraServicio obraServicio;
        private readonly IUsuarioServicio usuarioServicio;

        public ObraUsuarioServicio(AppDbContext baseDeDatos)
        {
            this.baseDeDatos = baseDeDatos;
        }

        //public async Task<(bool, string)> AsignarUsuarioAObra(ObraAsociarUsuarioDTO ObraUsuarioDTO)
        //{
        //    try
        //    {
        //        var existeAsignacion = await baseDeDatos.Obras.AnyAsync(ou => ObraUsuarioDTO.NombreObra.ToLower() == ObraUsuarioDTO.NombreObra.ToLower() && ou.EmpresaId == ObraUsuarioDTO.EmpresaId);
        //        if (existeAsignacion)
        //        {
        //            return (false, "El usuario ya está asignado a esta obra.");
        //        }
        //        var nuevaAsignacion = new ObraUsuario
        //        {
        //            ObraId = ObraUsuarioDTO.ObraId,
        //            UsuarioId = ObraUsuarioDTO.UsuarioId
        //        };
        //        await baseDeDatos.ObraUsuarios.AddAsync(nuevaAsignacion);
        //        await baseDeDatos.SaveChangesAsync();
        //        return (true, "Usuario asignado a la obra exitosamente.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.InnerException?.Message ?? ex.Message}");
        //        return (false, "Error al asignar el usuario a la obra.");
        //    }
        //}

        public async Task<(bool, string)> RemoverUsuarioDeObra(int obraId, long usuarioId)
        {
            try
            {
                var asignacion = await baseDeDatos.ObraUsuarios
                    .FirstOrDefaultAsync(ou => ou.ObraId == obraId && ou.UsuarioId == usuarioId);
                if (asignacion == null)
                {
                    return (false, "El usuario no está asignado a esta obra.");
                }
                baseDeDatos.ObraUsuarios.Remove(asignacion);
                await baseDeDatos.SaveChangesAsync();
                return (true, "Usuario removido de la obra exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException?.Message ?? ex.Message}");
                return (false, "Error al remover el usuario de la obra.");
            }
        }
        public async Task<(bool, List<long>)> ObtenerUsuariosDeObra(int obraId)
        {
            try
            {
                var usuariosIds = await baseDeDatos.ObraUsuarios
                    .Where(ou => ou.ObraId == obraId)
                    .Select(ou => ou.UsuarioId)
                    .ToListAsync();
                return (true, usuariosIds);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException?.Message ?? ex.Message}");
                return (false, null);
            }
        }
        public async Task<(bool, List<long>)> ObtenerObrasConUsuario(long usuarioId)
        {
            try
            {
                var obrasIds = await baseDeDatos.ObraUsuarios
                    .Where(ou => ou.UsuarioId == usuarioId)
                    .Select(ou => ou.ObraId)
                    .ToListAsync();
                return (true, obrasIds);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException?.Message ?? ex.Message}");
                return (false, null);
            }
        }
    }
}
