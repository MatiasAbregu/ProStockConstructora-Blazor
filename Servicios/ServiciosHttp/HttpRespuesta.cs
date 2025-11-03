using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.ServiciosHttp
{
    public class HttpRespuesta<T>
    {
        public T? Respuesta { get; }
        public bool Error { get; }
        public HttpResponseMessage Mensaje { get; set; }

        public HttpRespuesta(T? Respuesta, bool Error, HttpResponseMessage Mensaje)
        {
            this.Respuesta = Respuesta;
            this.Error = Error;
            this.Mensaje = Mensaje;
        }
    }
}
