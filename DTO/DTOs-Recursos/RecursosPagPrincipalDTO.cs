using DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DTO.DTOs_MaterialesYmaquinarias
{
    public class RecursosPagPrincipalDTO
    {
        [JsonPropertyName("id")]
        public int RecursosId { get; set; }
        public string CodigoISO { get; set; }
        public string Nombre { get; set; }
        public string UnidadMedida { get; set; }
    }
}
