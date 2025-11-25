using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Recursos
{
    public class RecursosCrearDTO
    {

        [Required(ErrorMessage = "El código ISO es obligatorio")]
        public string CodigoISO { get; set; }

        [Required(ErrorMessage = "El nombre del recurso es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La unidad de medida es obligatoria.")]
        public long UnidadDeMedidaId { get; set; }

        [Required(ErrorMessage = "El tipo de material es obligatorio.")]
        public long TipoMaterialId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad no puede ser menos de 1.")]
        public int Cantidad { get; set; }
        [Required]
        public long EmpresaId { get; set; }
        [Required]
        public long DepositoId { get; set; }

    }
}
