using BD.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Remitos
{
    public class ActualizarRemitoDTO
    { 
        public long Id { get; set; }
        public string? NumeroRemito { get; set; }
        public long? NotaDePedidoId { get; set; }
        public long? DepositoOrigenId { get; set; }
        public long? DepositoDestinoId { get; set; }
        public EnumEstadoRemito EstadoRemito { get; set; } = EnumEstadoRemito.Pendiente;
        public DateTime? FechaEmision { get; set; }
        public DateTime? FechaRecepcion { get; set; }
    }
}
