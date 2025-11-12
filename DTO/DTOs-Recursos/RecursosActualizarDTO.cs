using DTO.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_MaterialesYmaquinarias
{
    public class RecursosActualizarDTO
    {
        [Required(ErrorMessage = "El Codigo ISO es obligatorio")]
        public string CodigoISO { get; set; }

        [Required(ErrorMessage = "El Nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El Tipo es obligatorio")]
        public EnumTipoMaterialoMaquina Tipo { get; set; }

        public string? TipoMaterial { get; set; } 
        public string? UnidadDeMedida { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
    }
}
