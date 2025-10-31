using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_NotaDePedido
{
    public class ActualizarNotaDePedidoDTO
    {
        public int NotaPedidoId { get; set; }
        public string NumeroNotaPedido { get; set; }
        public string Material { get; set; }
        public int Cantidad { get; set; }
        public int DepositoDestinoId { get; set; }
        public DateTime FechaEmision { get; set; }
    }
}
