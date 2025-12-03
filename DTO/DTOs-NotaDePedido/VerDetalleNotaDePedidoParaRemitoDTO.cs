using DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_NotaDePedido
{
    public class VerDetalleNotaDePedidoParaRemitoDTO
    {
        public long Id { get; set; }
        public long RecursoId { get; set; }
        public string Recurso { get; set; }
        public int Cantidad { get; set; }
        public int CantidadDespachada { get; set; }
    }
}
