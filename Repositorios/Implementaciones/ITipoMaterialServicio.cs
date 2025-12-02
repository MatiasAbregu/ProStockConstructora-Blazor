using DTO.DTOs_Recursos;
using DTO.DTOs_Response;

namespace Repositorios.Implementaciones
{
    public interface ITipoMaterialServicio
    {
        Task<Response<List<TipoMaterialDTO>>> ObtenerTiposDeMaterial(long empresaId);
        Task<Response<TipoMaterialDTO>> ObtenerTipoMaterialPorId(long Id);
        Task<Response<string>> TipoMaterialCargar(TipoMaterialDTO tipoMaterial , long empresaId);
        Task<Response<string>> TipoMaterialModificar(TipoMaterialDTO tipoMaterialDTO, long Id);
    }
}