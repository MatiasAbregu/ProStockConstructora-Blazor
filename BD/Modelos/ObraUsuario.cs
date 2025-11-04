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
    [Index(nameof(ObraId), nameof(UsuarioId), IsUnique = true)]
    public class ObraUsuario
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long ObraId { get; set; }
        public Obra Obra { get; set; }

        [Required]
        public long UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    } 
}
