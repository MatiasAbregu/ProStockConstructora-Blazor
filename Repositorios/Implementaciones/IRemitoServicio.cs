using DTO.DTOs_Remitos;
using DTO.DTOs_Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.Implementaciones
{
    public interface IRemitoServicio
    {
        Task<Response<string>> ObtenerNumeroRemitoSiguiente();
    }
}
