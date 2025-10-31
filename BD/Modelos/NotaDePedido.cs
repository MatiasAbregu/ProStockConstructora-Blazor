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
        public int Id { get; set; }
        public required string NumeroNotaPedido { get; set; }
        public required int DepositoDestinoId { get; set; }
        public Deposito DepositoDestino { get; set; }

        public required DateTime FechaEmision { get; set; }
        public EstadoNotaPedido Estado { get; set; } = EstadoNotaPedido.Pendiente;

        public required string UsuarioId {  get; set; }

        public Usuario Usuario { get; set; }

        // HACER EN DTO
        //public List<DetalleNotaDePedido>? ListaDePedido { get; }
    }
}