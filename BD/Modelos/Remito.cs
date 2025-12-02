using BD.Enums;
using BD.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Modelos
{
    [Index(nameof(NumeroRemito), IsUnique = true)]
    public class Remito
    {
        [Key]
        public long Id { get; set; }
        public required string NumeroRemito { get; set; }

        public required long NotaDePedidoId { get; set; }
        public NotaDePedido NotaDePedido { get; set; }

        public required long DepositoOrigenId { get; set; }
        public Deposito Deposito { get; set; }

        public required long DepositoDestinoId { get; set; }
        public Deposito Destino { get; set; }

        public EnumEstadoRemito Estado { get; set; } = EnumEstadoRemito.Pendiente;

        public required DateTime FechaEmision { get; set; }
        public DateTime? FechaLimite { get; set; }
        public DateTime? FechaRecepcion { get; set; }

        // HACER EN DTO
        //public List<DetalleRemito>? ListaDelRemito { get; }

    }
}