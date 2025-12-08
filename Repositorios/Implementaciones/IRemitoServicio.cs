using DTO.DTOs_Remitos;
using DTO.DTOs_Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.DTOs_Usuarios;

namespace Repositorios.Implementaciones
{
    public interface IRemitoServicio
    {
        Task<Response<string>> ObtenerNumeroRemitoSiguiente();
        Task<Response<NotaDePedidoParaRemitoDTO>> ObtenerNotaDePedidoParaRemito(long NotaDePedidoId, long DepositoId);
        Task<Response<List<VerRemitoDTO>>> ObtenerRemitosPorNotaDePedido(long NotaDePedidoId, long DepositoId);
        Task<Response<VerRemitoDetalladoDTO>> ObtenerDetallesRemitoPorId(long RemitoId);
        Task<Response<List<VerRemitoDTO>>> ObtenerRemitosPendietesPorNotaDePedido(long NotaDePedidoId);
        Task<Response<List<VerRemitoDTO>>> ObtenerRemitosPendientes(DatosUsuario usuario);
        Task<Response<string>> CrearRemito(CrearRemitoDTO remitoDTO);
        Task<Response<string>> ActualizarEstadosRemito(long RemitoId, List<VerDetalleRemitoDTO> detalles);
        Task<Response<string>> AnularRemito(long RemitoId, long UsuarioId);
    }
}
