using DTO.DTOs_MaterialesYmaquinarias;
using DTO.DTOs_Recursos;
using DTO.DTOs_Response;

namespace Repositorios.Implementaciones
{
    public interface IRecursosServicio
    {
        Task<Response<string>> RecursoCargar(RecursosCargarDTO recursosCargarDTO, long depositoId);
        Task<(bool, string)> RecursosTransladarAdeposito(RecursosTransladarDepositoDTO RecursosTransladarDepositoDTO);
        Task<Response<List<RecursosPagPrincipalDTO>>> RecursosVerDTO(int empresaId);
        Task<(bool, List<RecursosVerDepositoDTO>)> RecursosVerDepositoDTO(int depositoId);
        Task<(bool, RecursoStockVerDTO)> ObtenerRecursoPorStockId(int stockId);
        Task<(bool, string)> RecursoEliminarStock(int stockId);
        Task<(bool, object)> VerificarRecursoPorCodigoISO(string codigoISO);
        Task<(bool, string)> RecursosActualizar(RecursosActualizarDTO recursoActualizarDTO, long recursoId);
    }
}