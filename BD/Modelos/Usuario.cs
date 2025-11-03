using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using BD.Modelos;
using Microsoft.EntityFrameworkCore;

namespace BD.Modelos
{
    [Index(nameof(NombreUsuario), nameof(EmpresaId), IsUnique = true)]
    public class Usuario
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public required string NombreUsuario { get; set; }

        [Required]
        public required string Contrasena { get; set; }

        [Required]
        public bool Estado { get; set; }

        [Required]
        public long EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}