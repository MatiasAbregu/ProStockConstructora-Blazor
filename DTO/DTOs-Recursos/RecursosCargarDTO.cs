using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.DTOs_Recursos;
using DTO.Enum;

namespace DTO.DTOs_MaterialesYmaquinarias
{
    public class RecursosCargarDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El Codigo ISO es obligatorio")]
        public string CodigoISO { get; set; }

        [Required(ErrorMessage = "El Nombre es obligatorio")]
        public string Nombre { get; set; }
        public TipoMaterialDTO? TipoMaterial { get; set; }
        public UnidadDeMedidaDTO? UnidadDeMedida { get; set; }

        [Required(ErrorMessage = "La Cantidad es obligatoria")]
        public int Cantidad { get; set; }
        public string? Descripcion { get; set; }
    }
}
