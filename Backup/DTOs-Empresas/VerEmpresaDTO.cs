using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.DTO.DTOs_Empresas
{
    public class VerEmpresaDTO
    {
        public int Id { get; set; }
        public string CUIT { get; set; }
        public string Nombre { get; set; } 
        public string RazonSocial { get; set; }
        public string Estado { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
    }
}
