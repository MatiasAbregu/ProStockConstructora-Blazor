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
        public Task<Response<string>> ActualizarRemito(long id, ActualizarRemitoDTO remitoDTO);
        public Task<Response<string>> CrearRemito(CrearRemitoDTO remitoDTO);
        public Task<Response<ActualizarRemitoDTO>> ObtenerRemitoPorId(long id);
        public Task<Response<ActualizarRemitoDTO>> ObtenerRemitoPorNotaDePedidoId(long notaDePedidoId);
    }
}
