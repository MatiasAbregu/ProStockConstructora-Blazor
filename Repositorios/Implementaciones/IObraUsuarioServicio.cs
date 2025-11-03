using DTO.DTOs_Obras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.Implementaciones
{
    public interface IObraUsuarioServicio
    {
        Task<(bool, string)> AsignarUsuarioAObra(ObraAsociarUsuarioDTO ObraUsuarioDTO);
        Task<(bool, string)> RemoverUsuarioDeObra(int obraId, long usuarioId);
        Task<(bool, List<long>)> ObtenerUsuariosDeObra(int obraId);
        Task<(bool, List<long>)> ObtenerObrasConUsuario(long usuarioId);
    }
}
