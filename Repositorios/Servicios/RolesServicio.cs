using BD;
using DTO.DTOs_Roles;
using Repositorios.Implementaciones;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.DTOs_Response;
using BD.Modelos;

namespace Repositorios.Servicios
{
    public class RolesServicio : IRolesServicio
    {
        private readonly AppDbContext baseDeDatos;

        public RolesServicio(AppDbContext baseDeDatos)
        {
            this.baseDeDatos = baseDeDatos;
        }

        public async Task<List<Rol>> BuscarRolesPorNombres(List<string> roles)
        {
            roles = roles.Select(r => r.ToUpper().Trim()).ToList();
            return await baseDeDatos.Roles.Where(rol => roles.Contains(rol.NombreRol)).ToListAsync();
        }

        public async Task<Response<List<VerRolDTO>>> ObtenerRoles()
        {
            try
            {
                var rolesPresentacion = new Dictionary<string, string>()
                {
                    { "ADMINISTRADOR", "Administrador" },
                    { "JEFEDEDEPOSITO", "Jefe de depósito" },
                    { "JEFEDEOBRA", "Jefe de obra" }
                };
                
                var roles = await baseDeDatos.Roles.Select(r => new VerRolDTO()
                {
                    NombreNormalizado = r.NombreRol,
                    NombreRol = rolesPresentacion[r.NombreRol]
                }).ToListAsync();

                return new Response<List<VerRolDTO>>()
                {
                    Objeto = roles,
                    Estado = true,
                    Mensaje = "¡Roles cargados con éxito!"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Response<List<VerRolDTO>>()
                {
                    Objeto = null,
                    Estado = false,
                    Mensaje = "¡Hubo un error desde el servidor al cargar los roles!"
                };
            }
        }
        
    }
}
