using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Usuarios
{
    public class ActualizarUsuarioDTO
    {
        [Required]
        public string Id { get; set; }

        public string? NombreUsuario { get; set; }
        public List<string> Roles { get; set; }
        public string? Email { get; set; } = null;
        public string? Celular { get; set; } = null;
    }
}
