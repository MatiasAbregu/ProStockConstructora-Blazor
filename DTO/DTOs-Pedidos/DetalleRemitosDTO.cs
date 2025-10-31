using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Pedidos
{
    public class DetalleRemitosDTO
    {
        public int RemitoId { get; set; }
        public int DetalleNotaDePedidoId { get; set; }
        public int CantidadRecibida { get; set; }
        public int CantidadDespachada { get; set; }
    }
}
