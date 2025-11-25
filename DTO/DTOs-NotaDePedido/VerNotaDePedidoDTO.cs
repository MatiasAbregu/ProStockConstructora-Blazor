using DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;


namespace DTO.DTOs_NotaDePedido
{
    public class VerNotaDePedidoDTO
    {
        public string NumeroNotaPedido { get; set; } = string.Empty;
        public long DepositoOrigenId { get; set; }
        public long DepositoDestinoId { get; set; }
        public DateTime FechaEmision { get; set; } = DateTime.Now;
        public EnumEstadoNotaPedido Estado { get; set; }= EnumEstadoNotaPedido.Pendiente;

        public List<DetalleNotaDePedidoDTO> ListaDePedido { get; set; } = new();
    }
}

