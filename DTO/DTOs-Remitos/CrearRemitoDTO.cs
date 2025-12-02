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
        public string NumeroRemito { get; set; }
        public long NotaDePedidoId { get; set; }
        public DateTime FechaEmision { get; set; }
    }
}
