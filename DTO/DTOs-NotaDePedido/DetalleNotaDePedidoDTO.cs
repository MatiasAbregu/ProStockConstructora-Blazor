using DTO.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_NotaDePedido
{
    public class DetalleNotaDePedidoDTO
    {
        [Required(ErrorMessage = "Es obligatorio seleccionar un deposito")]
        public long DepositoDestinoId { get; set; }

        [Required(ErrorMessage = "Es obligatorio seleccionar un recurso")]
        public long RecursoId { get; set; }
        [Range(0,int.MaxValue,ErrorMessage = "La cantidad no puede ser menor a 0")]
        public int Cantidad { get; set; }
        public EnumEstadoNotaPedido EstadoNotaPedido { get; set; } = EnumEstadoNotaPedido.Pendiente;
    }  
}
