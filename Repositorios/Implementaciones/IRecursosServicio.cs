using DTO.DTOs_MaterialesYmaquinarias;
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
        
        //Task<Response<string>> RecursosTransladarAdeposito(RecursosTransladarDepositoDTO RecursosTransladarDepositoDTO);      
        //Task<Response<List<RecursoStockVerDTO>>> ObtenerRecursoPorStockId(long stockId);
        //Task<Response<string>> RecursoEliminarStock(long stockId);
        //Task<Response<object>> VerificarRecursoPorCodigoISO(string codigoISO);
        //Task<Response<string>> RecursosActualizar(RecursosActualizarDTO recursoActualizarDTO, long recursoId);
    }
}