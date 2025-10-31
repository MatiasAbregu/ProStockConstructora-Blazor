using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Pedidos
{
    public class DetalleNotaDePedidoDTO
    {
        public int NotaDePedidoId { get; set; }
        public int RecursosId { get; set; }
        public int Cantidad { get; set; }
    }
}
