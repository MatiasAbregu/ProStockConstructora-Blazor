using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Obras
{
    public class ObraAsociarUsuarioDTO
    {
        public int ObraId { get; set; }
        public int EmpresaId { get; set; }
        public string NombreObra { get; set; }
        public required string UsuarioId { get; set; }
    }
}
