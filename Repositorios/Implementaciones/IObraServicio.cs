using DTO.DTOs_Obras;
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
        Task<(bool, List<VerObraDTO>)> ObtenerObras(int EmpresaId);
    }
}


