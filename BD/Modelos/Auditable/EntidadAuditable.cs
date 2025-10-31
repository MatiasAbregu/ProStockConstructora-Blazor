using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Modelos.Auditable
{
    public class EntidadAuditable
    {
        [Key]
        public long Id { get; set; }
    }
}
