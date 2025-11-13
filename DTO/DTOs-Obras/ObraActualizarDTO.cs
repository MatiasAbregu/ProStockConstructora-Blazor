using DTO.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Obras
{
    public class ObraActualizarDTO 
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "El código de obra es obligatorio.")]
        public string CodigoObra { get; set; }
        [Required(ErrorMessage = "El nombre de obra es obligatorio.")]
        public string NombreObra { get; set; }
        [Required(ErrorMessage = "El estado de la obra es obligatorio.")]
        public EnumEstadoObra Estado { get; set; }
    }
}

