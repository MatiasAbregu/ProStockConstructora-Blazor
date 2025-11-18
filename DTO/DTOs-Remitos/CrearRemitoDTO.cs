using BD.Enums;
using System;
using System.Collections.Generic;
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
        public EstadoRemito EstadoRemito { get; set; } = EstadoRemito.Pendiente;
        public string? NombreTransportista { get; set; }
        public required DateTime FechaEmision { get; set; }
        public required DateTime FechaLimite { get; set; }
        public DateTime? FechaRecepcion { get; set; }
    }
}
