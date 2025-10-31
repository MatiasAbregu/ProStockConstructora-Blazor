using DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Pedidos
{
    public class CrearNotaDePedidoDTO
    {
        public int DepositoDestinoId { get; set; }
        public string NumeroNotaPedido { get; set; }
        public DateTime FechaEmision { get; set; } = DateTime.Now;
        public EnumEstadoNotaPedido EstadoNotaPedido { get; set; } = EnumEstadoNotaPedido.Pendiente;
    }
}
