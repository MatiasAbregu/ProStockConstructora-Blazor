using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Recursos
{
    public class RecursoPorISODTO
    {
        [Required(ErrorMessage = "El código ISO es obligatorio.")]
        public string CodigoISO { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad no puede ser menos de 1.")]
        public int Cantidad { get; set; }
        [Required]
        public long DepositoId { get; set; }
        [Required]
        public long EmpresaId { get; set; }
    }
}
