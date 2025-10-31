using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Modelos
{
    public class Ubicacion
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El codigo de ubicacion es obligatorio.")]
        public required string CodigoUbicacion { get; set; }

        [Required(ErrorMessage = "El domicilio es obligatorio.")]
        public required string Domicilio { get; set; }

        public required int ProvinciaId { get; set; }
        public Provincia Provincia { get; set; }
    }
}
