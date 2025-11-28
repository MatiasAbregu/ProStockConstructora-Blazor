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
        public long Id { get; set; }
        public string NumeroNotaPedido { get; set; } 
        
        public DateTime FechaEmision { get; set; } 
        public EnumEstadoNotaPedido Estado { get; set; }

        public List<DetalleNotaDePedidoDTO> ListaDePedido { get; set; } = new();
    }
}

