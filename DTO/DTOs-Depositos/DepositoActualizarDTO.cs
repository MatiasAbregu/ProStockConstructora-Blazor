using DTO.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Depositos
{
    public class DepositoActualizarDTO
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "El código del depósito es obligatorio.")]
        public string CodigoDeposito { get; set; }
        [Required(ErrorMessage = "El nombre del depósito es obligatorio.")]
        public string NombreDeposito { get; set; }
        [Required(ErrorMessage = "El domicilio es obligatorio.")]
        public string Domicilio { get; set; }
        [Required(ErrorMessage = "Debe ser un tipo válido el depósito.")]
        public EnumTipoDeposito TipoDeposito { get; set; }
    }
}
