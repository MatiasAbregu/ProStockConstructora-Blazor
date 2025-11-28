using DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_NotaDePedido
{
    public class VerDetalleNotadePedidoDTO
    {
        public long Id { get; set; }
        public string Recurso { get; set; }
        public string Deposito { get; set; }
        public int Cantidad { get; set; }
        public EnumEstadoNotaPedido Estado { get; set; }
    }
}
