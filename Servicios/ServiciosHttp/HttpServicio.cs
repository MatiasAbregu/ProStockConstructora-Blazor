using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Servicios.ServiciosHttp
{
    public class HttpServicio : IHttpServicio
    {
        private readonly HttpClient http;

        public HttpServicio(HttpClient http)
        {
            this.http = http;
        }

        public async Task<HttpRespuesta<T>> Get<T>(string url)
        {
            var res = await http.GetAsync(url);
            var respuesta = await DesSerializar<T>(res);
            return new HttpRespuesta<T>(respuesta, false, res);
        }

        public async Task<HttpRespuesta<TResp>> Post<T, TResp>(string url, T entidad)
        {
            var contenido = Serializar(entidad);
            var res = await http.PostAsync(url, contenido);

            var respuesta = await DesSerializar<TResp>(res);
            return new HttpRespuesta<TResp>(respuesta, false, res);
        }

        public async Task<HttpRespuesta<TResp>> Put<T, TResp>(string url, T entidad)
        {
            var contenido = Serializar(entidad);
            var res = await http.PutAsync(url, contenido);

            var respuesta = await DesSerializar<TResp>(res);
            return new HttpRespuesta<TResp>(respuesta, false, res);
        }

        public async Task<HttpRespuesta<T>> Delete<T>(string url)
        {
            var res = await http.DeleteAsync(url);
            var respuesta = await DesSerializar<T>(res);
            return new HttpRespuesta<T>(respuesta, false, res);
        }

        private async Task<T?> DesSerializar<T>(HttpResponseMessage res)
        {
            var respStr = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(respStr, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });
        }

        private StringContent Serializar<T>(T entidad)
        {
            var JsonAEnviar = JsonSerializer.Serialize(entidad);
            return new StringContent(JsonAEnviar, Encoding.UTF8, "application/json");
        }
    }
}
