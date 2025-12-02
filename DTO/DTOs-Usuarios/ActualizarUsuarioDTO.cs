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
        public long Id { get; set; }
        [Required(ErrorMessage = "El correo es obligatorio.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string NombreUsuario { get; set; }
        public string? Contrasena { get; set; }
        public string? Celular { get; set; } = null;
        public long EmpresaId { get; set; }
        [Required(ErrorMessage = "El usuario debe contar con un rol.")]
        public List<string> Roles { get; set; }
        public List<long>? ObrasId { get; set; }
        public List<long>? DepositosId { get; set; }
    }
}
