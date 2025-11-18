using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Modelos
{
    [Index(nameof(Nombre), nameof(EmpresaId), IsUnique = true)]
    public class TipoMaterial
    {
        [Key]
        public long Id { get; set; }
        public required string Nombre { get; set; }
        public required long EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}
