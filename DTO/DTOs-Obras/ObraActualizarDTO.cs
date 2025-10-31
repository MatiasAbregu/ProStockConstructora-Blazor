using DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Obras
{
    public class ObraActualizarDTO 
    {
        public int Id { get; set; }
        public string CodigoObra { get; set; }
        public string NombreObra { get; set; }
        public EnumEstadoObra Estado { get; set; }
    }
}

