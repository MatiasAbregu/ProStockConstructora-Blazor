using DTO.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_NotaDePedido
{
    public class CrearNotaDePedidoDTO
    {
        [Required(ErrorMessage ="El numero de nota de pedido es obligatorio")]
        public string NumeroNotaPedido { get; set; } 

        [Required]
        public long UsuarioId { get; set; }

        [Required]
        public long DepositoOrigenId { get; set; }

        [Required]

        public List<DetalleNotaDePedidoDTO> ListaDePedido { get; set; }
    }
}
