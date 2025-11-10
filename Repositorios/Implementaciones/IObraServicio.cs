using DTO.DTOs_Obras;
using DTO.DTOs_Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.Implementaciones
{
    public interface IObraServicio
    {
        Task<(bool, string)> CrearObra(CrearObraDTO obraDTO); // habria que pasarle el id de la empresa
        Task<(bool, string)> ActualizarObra(int id, ObraActualizarDTO o);  
        Task<Response<VerObraDTO>> ObtenerObraPorId(int obraId); // seria obtener obra por CODIGO de OBRA
        Task<Response<VerObraDTO>> ObtenerObrasPorCodigoObra(string CodigoObra); //obras de la empresa por nombre o codigo
        Task<Response<List<VerObraDTO>>> ObtenerObras(int EmpresaId); //obras de la empresa
    }
}


