using BD.Enums;
using BD.Modelos;
using DTO.DTOs_Movimientos;
using DTO.DTOs_Response;

namespace Repositorios.Implementaciones;

public interface IMovimientoServicio
{
    Task<Response<List<VerMovimientoDTO>>> ObtenerMovimientosPorEmpresa(long EmpresaId);

    Task MovimientoStockEntreDepositos(Deposito depositoDestino, MovimientoStock movimiento, EnumEstadoRemito estado, int cantidadRecibida);
}