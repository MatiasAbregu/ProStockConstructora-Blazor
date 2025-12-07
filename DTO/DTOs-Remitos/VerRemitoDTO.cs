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
        public string NumeroRemito { get; set; }
        public EnumEstadoRemito Estado { get; set; } = EnumEstadoRemito.Emitido;
        public DateTime FechaEmision { get; set; }
    }
}
