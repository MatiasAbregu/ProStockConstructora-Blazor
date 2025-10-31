using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Modelos
{
    public class MovimientoDeposito
    {
        [Key]
        public int Id { get; set; }
        public int DepositoOrigenId { get; set; }
        [Required]
        public Deposito DepositoOrigen { get; set; }

        [Required]
        public int DepositoDestinoId { get; set; }
        public Deposito DepositoDestino { get; set; }

        [Required]
        public int MaterialOMaquinaId { get; set; }
        public Recursos MaterialOMaquina { get; set; }

        [Required]
        public int Cantidad { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

    }
}
