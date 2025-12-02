using DTO.DTOs_Depositos;
using DTO.DTOs_Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Stock
{
   public class VerStockDTO
    {
        public VerDepositoDTO DepositoDTO { get; set; } 

        public List<RecursosVerDTO> ListadeRecursos { get; set; }
    }

}
