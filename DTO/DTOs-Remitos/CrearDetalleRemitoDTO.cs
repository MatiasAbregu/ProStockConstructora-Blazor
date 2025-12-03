using BD.Enums;
using BD.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Remitos
{
    public class CrearDetalleRemitoDTO
    {
        public long Id { get; set; }
        public long RemitoId { get; set; }
        public long DetalleNotaDePedidoId { get; set; }
        public int CantidadDespachada { get; set; }
        public EnumEstadoRemito Estado { get; set; } = EnumEstadoRemito.EnTransito;
    }
}
