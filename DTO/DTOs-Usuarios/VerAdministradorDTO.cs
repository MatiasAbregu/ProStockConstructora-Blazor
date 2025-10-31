using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Usuarios
{
    public class VerAdministradorDTO
    {
        public string Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Estado {  get; set; }

        public int EmpresaId { get; set; }
        public string NombreEmpresa { get; set; }
    }
}

//[
//    {
//        "empresaId": 1,
//        "empresa": {
//            "id": 1,
//            "nombreEmpresa": "Pepe S.A.",
//            "cuit": "123456789",
//            "razonSocial": "Sociedad Anonima",
//            "estado": true,
//            "contactoId": 1,
//            "contacto": null
//    }
//]