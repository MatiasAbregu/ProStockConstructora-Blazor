using DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_NotaDePedido
{
    public class DetalleNotaDePedidoDTO
    {
        public long DepositoDestinoId { get; set; }
        public long RecursoId { get; set; }
        public int Cantidad { get; set; }
        public EnumEstadoNotaPedido EstadoNotaPedido { get; set; } = EnumEstadoNotaPedido.Pendiente;
    }  
}
