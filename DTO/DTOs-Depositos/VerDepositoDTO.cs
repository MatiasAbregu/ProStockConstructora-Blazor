using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Depositos
{
    public class VerDepositoDTO
    {
        public long Id { get; set; }
        public string CodigoDeposito { get; set; }
        public string NombreDeposito { get; set; }
        public string TipoDeposito { get; set; }
    }
}
