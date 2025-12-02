using DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Recursos
{
    public class RecursoStockVerDTO
    {
        public long StockId { get; set; }
        public long IdMaterial { get; set; }
        public string CodigoISO { get; set; }
        public EnumTipoMaterialoMaquina TipoRecurso { get; set; }
        public string Nombre { get; set; }
        public string? TipoMaterial { get; set; }
        public string? UnidadDeMedida { get; set; }
        public string? Descripcion { get; set; }
        public int Cantidad { get; set; }
    }
}
