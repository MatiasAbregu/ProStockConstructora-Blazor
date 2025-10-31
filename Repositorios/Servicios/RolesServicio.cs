using BD;
using DTO.DTOs_Roles;
using Repositorios.Implementaciones;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.Servicios
{
    public class RolesServicio : IRolesServicio
    {
        private readonly AppDbContext baseDeDatos;

        public RolesServicio(AppDbContext baseDeDatos)
        {
            this.baseDeDatos = baseDeDatos;
        }

        public (bool, List<VerRol>) ObtenerRoles()
        {
            //var rolesListado = gestorRoles.Roles.Where(r => r.NormalizedName != "SUPERADMINISTRADOR")
            //    .Select(r =>
            //    new VerRol()
            //    {
            //        NormalizedName = r.NormalizedName,
            //        Name = r.Name    
            //    }).ToList();
            //if (rolesListado == null || rolesListado.Count == 0)
            //{
            //    return (false, null);
            //} else return (true, rolesListado);
            throw new NotImplementedException();
        }
    }
}
