using DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_MaterialesYmaquinarias
{
    public class RecursosActualizarDTO
    {
        public int RecursoId { get; set; }
        public string CodigoISO { get; set; }
        public string Nombre { get; set; }
        public EnumTipoMaterialoMaquina Tipo { get; set; }
        public string? TipoMaterial { get; set; } 
        public string? UnidadDeMedida { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
    }
}
