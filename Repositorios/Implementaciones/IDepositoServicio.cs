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
        public Task<Response<List<VerDepositoDTO>>> ObtenerDepositoPorId(int id);
        public Task<Response<string>> CrearDeposito(DepositoAsociarDTO e);
        public Task<Response<string>> ActualizarDeposito(int id, DepositoAsociarDTO e);
        public Task<Response<string>> EliminarDeposito(long id);
        Task<Response<List<VerDepositoDTO>>> ObtenerDepositosPorUsuario(DatosUsuario usuario, long? ObraId);
    }
}
