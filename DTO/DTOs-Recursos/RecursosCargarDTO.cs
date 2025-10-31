using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.DTOs_Recursos;
using DTO.Enum;

namespace DTO.DTOs_MaterialesYmaquinarias
{
    public class RecursosCargarDTO
    {
        public string CodigoISO { get; set; }
        public string Nombre { get; set; }
        public EnumTipoMaterialoMaquina Tipo { get; set; }
        public TipoMaterialDTO? TipoMaterial { get; set; }
        public UnidadDeMedidaDTO? UnidadDeMedida { get; set; }
        public int Cantidad { get; set; }
        public string? Descripcion { get; set; }
    }
}
