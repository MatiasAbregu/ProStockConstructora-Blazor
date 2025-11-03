using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Modelos
{
    [Index(nameof(UsuarioId), nameof(DepositoId), IsUnique = true)]
    public class DepositoUsuario
    {
        [Key]
        public long Id { get; set; }

        public long UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public long DepositoId { get; set; }
        public Deposito Deposito { get; set; }
    }
}
