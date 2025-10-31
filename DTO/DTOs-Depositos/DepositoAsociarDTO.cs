using DTO.DTOs_Ubicacion;
using DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Depositos
{
    public class DepositoAsociarDTO
    {
        public int Id { get; set; }
        public string CodigoDeposito { get; set; }
        public string NombreDeposito { get; set; }
        public int ObraId { get; set; }
        public EnumTipoDeposito TipoDeposito { get; set; }
        public UbicacionDTO Ubicacion { get; set; }
    }
}
