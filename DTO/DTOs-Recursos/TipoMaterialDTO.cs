using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Recursos
{
    public class TipoMaterialDTO
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "El nombre del tipo de material es obligatorio.")]
        public string Nombre { get; set; }
        public long EmpresaId { get; set; }
    }
}
