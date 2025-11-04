using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Obras
{
    public class ObraAsociarUsuarioDTO
    {
        public long ObraId { get; set; }
        public long EmpresaId { get; set; }
        public string NombreObra { get; set; }
        public required long UsuarioId { get; set; }
    }
}
