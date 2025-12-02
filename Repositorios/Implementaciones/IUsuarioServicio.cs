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

        // POSTs
        public Task<Response<DatosUsuario>> IniciarSesion(IniciarSesionDTO usuarioDTO);
        public Task<Response<string>> CrearUsuario(CrearUsuarioDTO usuario);

        // PUTs
        public Task<Response<string>> ActualizarUsuario(long id, ActualizarUsuarioDTO usuario);
        public Task<Response<string>> CambiarEstadoUsuario(long id);
    }
}
