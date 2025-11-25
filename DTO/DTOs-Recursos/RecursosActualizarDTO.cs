using DTO.DTOs_Recursos;
using DTO.DTOs_Response;
using DTO.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace DTO.DTOs_MaterialesYmaquinarias
{
    public class RecursosActualizarDTO
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "El código ISO es obligatorio")]
        public string CodigoISO { get; set; }

        [Required(ErrorMessage = "El nombre del recurso es obligatorio")]
        public string Nombre { get; set; }

        [Required]
        public long TipoMaterialId { get; set; }
        [Required]
        public long UnidadDeMedidaId { get; set; }

        [Required]
        public long DepositoId { get; set; }
        public long? StockId { get; set; }
        public int? Cantidad { get; set; } = 0;
    }
}
