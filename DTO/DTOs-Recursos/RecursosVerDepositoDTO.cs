using DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_MaterialesYmaquinarias
{
    public class RecursosVerDepositoDTO
    {
        public int Id { get; set; }
        public string CodigoISO { get; set; }   
        public string Nombre { get; set; }
        public string TipoRecursoTipoMaterial { get; set; }
        public string UnidadMedida { get; set; }
        public int Cantidad { get; set; }
    }
}
