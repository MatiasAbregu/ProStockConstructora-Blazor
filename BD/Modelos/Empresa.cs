using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Modelos
{
    [Index(nameof(CUIT), IsUnique = true)]
    public class Empresa
    {
        [Key]
        public long Id { get; set; }

        [Column(TypeName = "varchar(150)")]
        public required string NombreEmpresa { get; set; }

        [Column(TypeName = "varchar(20)")]
        public required string CUIT { get; set; }

        [Column(TypeName = "varchar(150)")]
        public required string RazonSocial { get; set; }

        [Column(TypeName = "tinyint(1)")]
        [Required(ErrorMessage = "El estado es obligatorio: 0 es inactivo / 1 es activo.")]
        public bool Estado { get; set; } = true;

        [Column(TypeName = "varchar(80)")]
        public string? Celular { get; set; } = null;

        [EmailAddress(ErrorMessage = "El email no es válido.")]
        [Column(TypeName = "varchar(120)")]
        public string? Email { get; set; } = null;
    }
}
