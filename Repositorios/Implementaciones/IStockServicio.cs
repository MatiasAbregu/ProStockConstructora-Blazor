using DTO.DTOs_Response;
using DTO.DTOs_Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.Implementaciones
{
    public interface IStockServicio
    {
        Task<Response<List<VerStockDTO>>> ObtenerStockPorObrasId(List<long> ObrasId, long DepositoOrigenId);
        Task<Response<List<VerStockDTO>>> ObtenerStocksDeEmpresaPorIdAdministrador(long UsuarioId, long DepositoOrigenId);
    }
}
