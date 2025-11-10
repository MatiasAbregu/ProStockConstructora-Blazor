using BD.Modelos;
using DTO.DTOs_Recursos;
using DTO.DTOs_Response;

namespace Repositorios.Implementaciones
{
    public interface IUnidadMedidaServicio
    {
        Task<Response<List<UnidadDeMedidaDTO>>> ObtenerUnidadesDeMedida();
        Task<Response<string>> UnidadDeMedidaCargar(UnidadDeMedidaDTO unidadDeMedidaDTO);
        Task<Response<string>> UnidadDeMedidaModificar(UnidadDeMedidaDTO unidadDeMedidaDTO, long Id);
    }
}