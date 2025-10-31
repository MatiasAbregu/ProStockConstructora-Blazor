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
    public class Usuario
    {
        [Required]
        public string NombreUsuario { get; set; }

        [Required]
        public bool Estado { get; set; }

        [Required]
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}