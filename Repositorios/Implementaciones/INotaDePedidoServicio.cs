using DTO.DTOs_NotaDePedido;
using DTO.DTOs_Response;
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
    }
}
