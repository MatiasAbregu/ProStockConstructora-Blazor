using BD.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Modelos
{
    [Index(nameof(CodigoDeposito), IsUnique = true)]
    public class Deposito
    {
        [Key]
        public int Id { get; set; }

        public string CodigoDeposito { get; set; }
        public string NombreDeposito { get; set; }

        public EnumTipoDeposito TipoDeposito { get; set; } = EnumTipoDeposito.Disponible;

        [Required(ErrorMessage = "La obra del deposito es obligatorio.")]
        public required int ObraId { get; set; }
        public Obra Obra { get; set; }

        [Required(ErrorMessage = "La ubicacion del deposito es obligatorio.")]
        public required int UbicacionId { get; set; }
        public Ubicacion Ubicacion { get; set; }
    }
}