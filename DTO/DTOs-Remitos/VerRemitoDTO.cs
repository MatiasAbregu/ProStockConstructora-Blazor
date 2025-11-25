using BD.Enums;
using BD.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Remitos
{
    public class VerRemitoDTO
    {
        public long Id { get; set; }
        public required string NumeroRemito { get; set; }
        public required long NotaDePedidoId { get; set; }
        public required long DepositoOrigenId { get; set; }
        public required long DepositoDestinoId { get; set; }
        public EnumEstadoRemito EstadoRemito { get; set; } = EnumEstadoRemito.Pendiente;
        public required DateTime FechaEmision { get; set; }
        public DateTime? FechaRecepcion { get; set; }
        public List<DetalleRemito>? ListaDelRemito { get; }
    }
}
