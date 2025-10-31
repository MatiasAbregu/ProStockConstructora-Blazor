using DTO.DTOs_Depositos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.Implementaciones
{
    public interface IDepositoServicio
    {
        public Task<(bool, List<VerDepositoDTO>)> ObtenerDepositosPorObraId(int obraId);
        public Task<(bool, VerDepositoDTO)> ObtenerDepositoPorId(int id);
        public Task<(bool, string)> CrearDeposito(DepositoAsociarDTO e);
        public Task<(bool, string)> ActualizarDeposito(DepositoAsociarDTO e);
        public Task<(bool, string)> EliminarDeposito(int id);

    }
}
