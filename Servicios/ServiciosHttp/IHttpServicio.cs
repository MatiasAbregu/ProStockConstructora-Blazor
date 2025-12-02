
namespace Servicios.ServiciosHttp
{
    public interface IHttpServicio
    {    
        Task<HttpRespuesta<T>> Get<T>(string url);
        Task<HttpRespuesta<TResp>> Post<T, TResp>(string url, T entidad);
        Task<HttpRespuesta<TResp>> Put<T, TResp>(string url, T entidad);
        Task<HttpRespuesta<T>> Delete<T>(string url);
    }
}