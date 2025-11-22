using DTO.DTOs_MaterialesYmaquinarias;
using DTO.DTOs_Recursos;
using DTO.DTOs_Response;

namespace Repositorios.Implementaciones
{
    public interface IRecursosServicio
    {
        Task<Response<string>> RecursoCargar(RecursosCargarEmpresaDTO recursosCargarDTO, long depositoId);
        Task<Response<string>> RecursosTransladarAdeposito(RecursosTransladarDepositoDTO RecursosTransladarDepositoDTO);
        Task<Response<List<RecursosPagPrincipalDTO>>> RecursosVerDTO(long empresaId);
        Task<Response<List<RecursosVerDepositoDTO>>> RecursosVerDepositoDTO(long depositoId);
        Task<Response<List<RecursoStockVerDTO>>> ObtenerRecursoPorStockId(long stockId);
        Task<Response<string>> RecursoEliminarStock(long stockId);
        Task<Response<object>> VerificarRecursoPorCodigoISO(string codigoISO);
        Task<Response<string>> RecursosActualizar(RecursosActualizarDTO recursoActualizarDTO, long recursoId);
    }
}