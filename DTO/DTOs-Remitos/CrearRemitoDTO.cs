using BD.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Remitos
{
    public class CrearRemitoDTO
    {
        public required string NumeroRemito { get; set; }
        public required long NotaDePedidoId { get; set; }
        public required long DepositoOrigenId { get; set; }
        public required long DepositoDestinoId { get; set; }
        public EnumEstadoRemito EstadoRemito { get; set; } = EnumEstadoRemito.Pendiente;
        public required DateTime FechaEmision { get; set; }
        public DateTime? FechaRecepcion { get; set; }
    }
}
