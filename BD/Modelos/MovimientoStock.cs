using BD.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Modelos
{
    public class MovimientoStock
    {
        public long Id { get; set; }
        public long DetalleRemitoId { get; set; }
        public DetalleRemito DetalleRemito { get; set; }
        public int Cantidad { get; set; }
        public TipoDeMovimiento TipoDeMovimiento { get; set; }
    }
}
