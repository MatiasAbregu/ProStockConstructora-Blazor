using BD.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Modelos
{
    [Index(nameof(CodigoObra), IsUnique = true)]
    public class Obra
    {
        [Key]
        public long Id { get; set; }

        public string CodigoObra { get; set; }

        [MaxLength(100)]
        public required string NombreObra { get; set; }

        public EnumEstadoObra Estado { get; set; } = EnumEstadoObra.EnProceso;

        public required long EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

    }
}
