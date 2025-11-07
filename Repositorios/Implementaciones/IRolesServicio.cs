using DTO.DTOs_Response;
using DTO.DTOs_Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.Implementaciones
{
    public interface IRolesServicio
    {
        public Task<Response<List<VerRolDTO>>> ObtenerRoles();
    }
}
