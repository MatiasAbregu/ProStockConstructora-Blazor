using DTO.DTOs_Depositos;
using DTO.DTOs_Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.Implementaciones
{
    public interface IDepositoServicio
    {
        public Task<Response<List<DepositoEmpresaDTO>>> ObtenerDepositosDeEmpresa(long EmpresaId);
        public Task<Response<List<VerDepositoDTO>>> ObtenerDepositosPorObraId(int obraId);
        public Task<Response<List<VerDepositoDTO>>> ObtenerDepositoPorId(int id);
        public Task<Response<int>>CrearDeposito(DepositoAsociarDTO e);
        public Task<Response<string>> ActualizarDeposito(int id,DepositoAsociarDTO e);
        public Task<Response<string>> EliminarDeposito(long id);

    }
}
