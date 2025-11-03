using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_NotaDePedido
{
    public class DetalleNotaDePedidoDTO
    {
        public required long MaterialesyMaquinasId { get; set; }
        public required int Cantidad { get; set; }
    }  
}
