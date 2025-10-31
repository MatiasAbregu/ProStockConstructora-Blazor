using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Modelos
{
    public class DetalleNotaDePedido
    {
        [Key]
        public int Id { get; set; }

        public required int NotaDePedidoId { get; set; }
        public NotaDePedido NotaDePedido { get; set; }

        public required int MaterialesyMaquinasId { get; set; }
        public Recursos MaterialesyMaquinas { get; set; }

        public required int Cantidad { get; set; } 
    }
}