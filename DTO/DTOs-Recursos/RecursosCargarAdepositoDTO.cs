using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Recursos
{
    public class RecursosCargarAdepositoDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El Codigo ISO es obligatorio")]
        public string CodigoISO { get; set; }

        [Range(1,int.MaxValue,ErrorMessage ="No puede ser menos de 1 el valor.")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El deposito es obligatorio")]
        public long DepositoId { get; set; }
    }
}
