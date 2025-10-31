using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Modelos
{
    public class DetalleRemito
    {
        [Key]
        public int Id { get; set; } 

        public required int RemitoId { get; set; }
        public Remito Remito { get; set; }

        public required int DetalleNotaDePedidoId { get; set; }
        public DetalleNotaDePedido DetalleNotaDePedido { get; set; }

        public required int CantidadDespachada { get; set; }
        public int CantidadRecibida { get; set; }
    }
}