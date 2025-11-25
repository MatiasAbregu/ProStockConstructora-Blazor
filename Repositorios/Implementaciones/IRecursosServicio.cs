using DTO.DTOs_Recursos;
using DTO.DTOs_Response;

namespace Repositorios.Implementaciones
{
    public interface IRecursosServicio
    {
        // GETs
        Task<Response<List<RecursosVerDTO>>> ObtenerRecursosEmpresa(long empresaId);
        Task<Response<List<RecursosVerDTO>>> ObtenerRecursosDeposito(long depositoId);
        Task<Response<RecursosActualizarDTO>> ObtenerRecursoPorIdYODeposito(long? DepositoId, long RecursoId);

        // POSTs
        Task<Response<string>> RecursoCrear(RecursosCrearDTO recursoDTO);
        Task<Response<string>> RecursoAnadirPorISO(RecursoPorISODTO recursoDTO);

        // PUTs
        Task<Response<string>> RecursoActualizar(long? DepositoId, long RecursoId, RecursosActualizarDTO recursoDTO);

    }
}