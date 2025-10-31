using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Pedidos
{
    public class CrearRemitoDTO
    {
        public int NotaDePedidoId { get; set; }
        public int DepositoOrigenId { get; set; }
        public string EstadoRemito { get; set; } = "Pendiente";
        public string? NombreTransportista { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaLimite { get; set; }
        public DateTime? FechaSalida { get; set; }
        public DateTime? FechaRecepcion { get; set; }
        public string? RecibidoPor { get; set; }
        public int UsuarioId { get; set; }
    }
}
