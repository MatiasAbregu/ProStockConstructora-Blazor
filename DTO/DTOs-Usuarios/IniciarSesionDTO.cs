using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Usuarios
{
    public class IniciarSesionDTO
    {
        [Required(ErrorMessage = "Ingrese el nombre de usuario antes de continuar.")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "Ingrese la contraseña antes de continuar.")]
        public string Contrasena { get; set; }
    }
}
