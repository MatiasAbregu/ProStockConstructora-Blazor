using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Ubicacion
{
    public class UbicacionDTO 
    {
        public int Id { get; set; } = 0;
        public string? CodigoUbicacion { get; set; }
        public string? UbicacionDomicilio { get; set; }
        public ProvinciaDTO? Provincia { get; set; }

    }
}
