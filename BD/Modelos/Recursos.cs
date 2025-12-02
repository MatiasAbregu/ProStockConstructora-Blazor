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

    [Index(nameof(CodigoISO), nameof(EmpresaId), IsUnique = true)]
    public class Recursos
    {
        [Key]
        public long Id { get; set; }
        public required string CodigoISO { get; set; }

        public required string Nombre { get; set; }

        public long? UnidadMedidaId { get; set; }
        public UnidadMedida UnidadMedida { get; set; }

        public long? TipoMaterialId { get; set; }
        public TipoMaterial TipoMaterial { get; set; }

        public long EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}
