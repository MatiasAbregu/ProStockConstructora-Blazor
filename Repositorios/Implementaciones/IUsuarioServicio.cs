using BD.Modelos;
using DTO.DTOs_Response;
using DTO.DTOs_Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.Implementaciones
{
    public interface IUsuarioServicio
    {
        // GETs
        public Task<Response<List<DatosUsuario>>> ObtenerUsuariosPorEmpresaId(long id);
        public Task<Response<DatosUsuario>> ObtenerUsuarioPorId(long id);
        public Task<List<Usuario>> ObtenerUsuariosPorCategoria(); // Obra o Rol
        public Task<Usuario> ObtenerUsuarioPorNombreUsuario();

        // POSTs
        public Task<Response<DatosUsuario>> IniciarSesion(IniciarSesionDTO usuarioDTO);
        public Task<Response<string>> CrearUsuario(CrearUsuarioDTO usuario);

        // PUTs
        public Task<(bool, string, Usuario)> ActualizarUsuario(string id, ActualizarUsuarioDTO usuario);
        public Task<(bool, string)> CambiarEstadoUsuario(string id);
    }
}
