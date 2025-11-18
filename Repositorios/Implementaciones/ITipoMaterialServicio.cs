using DTO.DTOs_Recursos;
using DTO.DTOs_Response;

namespace Repositorios.Implementaciones
{
    public interface ITipoMaterialServicio
    {
        Task<Response<List<TipoMaterialDTO>>> ObtenerTiposDeMaterial();
        Task<Response<string>> TipoMaterialCargar(TipoMaterialDTO tipoMaterial);
        Task<Response<string>> TipoMaterialModificar(TipoMaterialDTO tipoMaterialDTO, long Id);
    }
}