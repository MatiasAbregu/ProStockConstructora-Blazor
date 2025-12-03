using BD.Modelos;
using DTO.DTOs_NotaDePedido;
using DTO.DTOs_Remitos;
using DTO.DTOs_Response;
using DTO.DTOs_Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.Implementaciones
{
    public interface INotaDePedidoServicio
    {
        Task<Response<string>> CrearNotaDePedido(CrearNotaDePedidoDTO NotadePedidoDTO);
        Task<Response<string>> ObtenerNumeroNotadePedidoSiguiente();
        Task<Response<List<VerNotaDePedidoDTO>>> ObtenerNotasDePedidoPorDepositoId(long DepositoId);
        Task<Response<VerNotadePedidoDetalladaDTO>> ObtenerDetallesNotaDePedidoPorId(long NotaDePedidoId);
        Task<Response<List<VerNotaDePedidoDTO>>> ObtenerNotasDePedidoPendientes(DatosUsuario Usuario);
        Task<Response<List<VerNotaDePedidoDTO>>> ObtenerNotasDePedidoPendientesPorDepositoId(long DepositoId);
        Task<Response<string>> ActualizarEstadosNotaDePedido(long NotaDePedidoId, List<VerDetalleNotadePedidoDTO> detalles);
        Task<Response<NotaDePedidoParaRemitoDTO>> ObtenerNotaDePedidoParaRemito(long NotaDePedidoId, long DepositoId);
        Task<Response<string>> AnularNotaDePedido(long NotaDePedidoId);
    }
}
