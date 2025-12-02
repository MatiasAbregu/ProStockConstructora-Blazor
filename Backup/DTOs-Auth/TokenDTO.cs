using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.DTO.DTOs_Usuarios
{
    public class TokenDTO
    {
        public string Mensaje { get; set; }
        public string? JWTToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
