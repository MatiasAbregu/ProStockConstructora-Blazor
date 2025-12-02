using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTOs_Response
{
    public class Response<T>
    {
        public T? Objeto { get; set; }
        public string? Mensaje { get; set; }
        public bool Estado { get; set; }
    }
}
