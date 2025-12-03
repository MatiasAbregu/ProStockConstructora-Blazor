using BD.Enums;
using BD.Modelos;
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
        public  string NumeroRemito { get; set; }
        public  long NotaDePedidoId { get; set; }
        public long DepositoOrigenId { get; set; }
        public long UsuarioId { get; set; }
        public  DateTime FechaEmision { get; set; } = DateTime.Now;
        public List<CrearDetalleRemitoDTO> DetallesDelRemito { get; set; }

    }
}
