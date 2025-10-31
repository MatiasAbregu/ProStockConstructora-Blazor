using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BD.Modelos;
using DTO.DTOs_Usuarios;

namespace Repositorios.Implementaciones
{
    public interface IUsuarioServicio
    {
        // GETs
        public Task<(bool, List<VerAdministradorDTO>)> ObtenerTodosLosAdministradores();
        public Task<List<Usuario>> ObtenerTodosLosAdministradoresDeEmpresa(string nombreEmpresa);
        public Task<(bool, List<VerUsuarioDTO>)> ObtenerUsuariosPorEmpresaId(int id);
        public Task<(bool, VerUsuarioDTO)> ObtenerUsuarioPorId(string id);
        public Task<List<Usuario>> ObtenerUsuariosPorCategoria(); // Obra o Rol
        public Task<Usuario> ObtenerUsuarioPorNombreUsuario();

        // POSTs
        //public Task<IdentityResult> CrearUsuario(CrearUsuarioDTO usuario);

        // PUTs
        public Task<(bool, string, Usuario)> ActualizarUsuario(string id, ActualizarUsuarioDTO usuario);
        public Task<(bool, string)> CambiarEstadoUsuario(string id);
    }
}
