using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Obras
{
    public class VerObraDTO
    {
        public int Id { get; set; }
        public string CodigoObra { get; set; }
        public string NombreObra { get; set; }
        public required string Estado { get; set; }
    }
}

