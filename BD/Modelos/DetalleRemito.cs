using BD.Enums;
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
        public long Id { get; set; } 

        public required long RemitoId { get; set; }
        public Remito Remito { get; set; }
        public required long DetalleNotaDePedidoId { get; set; }
        public DetalleNotaDePedido DetalleNotaDePedido { get; set; }
        public required int CantidadDespachada { get; set; }
        public int? CantidadRecibida { get; set; }
        public EnumEstadoRemito Estado { get; set; } = EnumEstadoRemito.Emitido;
        public long? UsuarioQueRecibeId { get; set; }
        public Usuario? UsuarioQueRecibe { get; set; }

    }
}