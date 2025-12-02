using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Enum;

namespace DTO.DTOs_NotaDePedido
{
    public class ActualizarDetalleNotaDePedidoDTO
    {
        public long Id { get; set; }
        public EnumEstadoNotaPedido Estado { get; set; }
        public long UsuarioQueModificoId { get; set; }
    }
}
