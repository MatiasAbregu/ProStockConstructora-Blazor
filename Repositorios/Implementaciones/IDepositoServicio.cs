using DTO.DTOs_Depositos;
using DTO.DTOs_Response;
using DTO.DTOs_Usuarios;
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
        public Task<Response<DepositoActualizarDTO>> ObtenerDepositoPorId(long id);
        public Task<Response<string>> CrearDeposito(DepositoAsociarDTO e);
        public Task<Response<string>> ActualizarDeposito(long id, DepositoAsociarDTO e);
        public Task<Response<List<VerDepositoDTO>>> ObtenerDepositosPorUsuario(DatosUsuario usuario, long? ObraId);
    }
}
