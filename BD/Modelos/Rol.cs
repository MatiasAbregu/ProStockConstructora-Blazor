using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Modelos
{
    public class Rol
    {
        [Key]
        public long Id { get; set; }

        private string _nombreRol;

        public string NombreRol
        {
            get => _nombreRol;
            set => _nombreRol = value.ToUpperInvariant();
        }
    }
}
