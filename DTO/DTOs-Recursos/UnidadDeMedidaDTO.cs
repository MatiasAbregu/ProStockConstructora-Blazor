using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Recursos
{
    public class UnidadDeMedidaDTO
    {
        [Required(ErrorMessage = "El nombre de la unidad de medida es obligatorio.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El simbolo de la unidad de medida es obligatorio.")]
        public string Simbolo { get; set; }
    }
}
