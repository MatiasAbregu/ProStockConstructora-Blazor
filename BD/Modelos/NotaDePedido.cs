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
    [Index(nameof(NumeroNotaPedido), IsUnique = true)]
    public class NotaDePedido
    {
        [Key]
        public long Id { get; set; }
        public required string NumeroNotaPedido { get; set; }

        public required long DepositoOrigenId { get; set; }
        public Deposito DepositoOrigen { get; set; }

        public required long UsuarioId { get; set; }
        public Usuario Usuario {  get; set; }

        public required DateTime FechaEmision { get; set; }
       

        // HACER EN DTO
        //public List<DetalleNotaDePedido>? ListaDePedido { get; }
    }
}