using DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_NotaDePedido
{
    public class VerNotaDePedidoDTO
    {
        public long NotaPedidoId { get; set; }
        public string NumeroNotaPedido { get; set; }
        public long DepositoDestinoId { get; set; }
        public DateTime FechaEmision { get; set; }
        public EnumEstadoNotaPedido Estado { get; set; }= EnumEstadoNotaPedido.Pendiente;

        public List<DetalleNotaDePedidoDTO> ListaDePedido { get; set; }
    }
}
