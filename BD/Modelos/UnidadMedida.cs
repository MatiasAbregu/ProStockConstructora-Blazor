using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Modelos
{
    public class UnidadMedida
    {
        [Key]
        public long Id { get; set; }
        public required string Nombre { get; set; } 
        public required string Simbolo { get; set; } 
    }
}
