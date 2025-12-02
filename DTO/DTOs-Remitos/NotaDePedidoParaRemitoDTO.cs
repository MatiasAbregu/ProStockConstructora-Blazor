using BD.Enums;
using DTO.DTOs_NotaDePedido;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Remitos
{
    public class NotaDePedidoParaRemitoDTO
    {
        public long NotaPedidoId { get; set; }
        public string DepositoDestino { get; set; }
        public List<VerDetalleNotaDePedidoParaRemitoDTO> detalle { get; set; }
    }
}
