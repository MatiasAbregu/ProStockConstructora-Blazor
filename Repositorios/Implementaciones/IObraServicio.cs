using DTO.DTOs_Obras;
using DTO.DTOs_Response;
using DTO.DTOs_Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.Implementaciones
{
    public interface IObraServicio
    {
        Task<(bool, string)> CrearObra(CrearObraDTO obraDTO);
        Task<(bool, string)> ActualizarObra(int id, ObraActualizarDTO o);
        Task<(bool, VerObraDTO)> ObtenerObraPorId(int id);
        Task<Response<List<ObraEmpresaDTO>>> ObtenerObrasDeEmpresa(long EmpresaId);
        Task<Response<List<VerObraDTO>>> ObtenerObrasPorUsuario(DatosUsuario usuario);
    }
}


