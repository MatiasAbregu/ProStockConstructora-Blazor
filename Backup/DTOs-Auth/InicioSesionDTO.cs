using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.DTO.DTOs_Usuarios
{
    public class InicioSesionDTO
    {
        [Required]
        public string NombreUsuario { get; set; }
        [Required]
        public string Contrasena { get; set; }
        public bool MantenerSesion { get; set; }
    }
}
