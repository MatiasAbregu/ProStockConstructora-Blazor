using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_NotaDePedido
{
    public class VerNotadePedidoDetalladaDTO
    {
        public long Id { get; set; }
        public string NumeroNotaPedido { get; set; }
        public string DepositoOrigen { get; set; }
        public string Usuario { get; set; }
        public DateTime FechaEmision { get; set; }
        public List<VerDetalleNotadePedidoDTO> Detalles { get; set; }
    }

}
