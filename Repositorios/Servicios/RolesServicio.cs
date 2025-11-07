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

namespace Repositorios.Servicios
{
    public class RolesServicio : IRolesServicio
    {
        private readonly AppDbContext baseDeDatos;

        public RolesServicio(AppDbContext baseDeDatos)
        {
            this.baseDeDatos = baseDeDatos;
        }

        public async Task<Response<List<VerRol>>> ObtenerRoles()
        {
            try
            {
                var rolesNormalizado = new Dictionary<string, string>()
                {
                    { "ADMINISTRADOR", "Administrador" },
                    { "JEFEDEDEPOSITO", "Jefe de depósito" },
                    { "JEFEDEOBRA", "Jefe de obra" }
                };
                
                var roles = await baseDeDatos.Roles.Select(r => new VerRol()
                {
                    Id = r.Id,
                    NombreRol = rolesNormalizado[r.NombreRol]
                }).ToListAsync();

                return new Response<List<VerRol>>()
                {
                    Objeto = roles,
                    Estado = true,
                    Mensaje = "¡Roles cargados con éxito!"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Response<List<VerRol>>()
                {
                    Objeto = null,
                    Estado = false,
                    Mensaje = "¡Hubo un error desde el servidor al cargar los roles!"
                };
            }
        }
    }
}
