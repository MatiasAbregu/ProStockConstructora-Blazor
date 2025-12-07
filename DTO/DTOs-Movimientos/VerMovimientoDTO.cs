using DTO.Enum;

namespace DTO.DTOs_Movimientos;

public class VerMovimientoDTO
{
    public long Id { get; set; }
    public long RemitoId { get; set; }
    public string NumeroRemito { get; set; }
    public long DepositoId { get; set; }
    public string Deposito { get; set; }
    public string Recurso { get; set; }
    public int Cantidad { get; set; }
    public TipoDeMovimiento TipoDeMovimiento { get; set; }
    public DateTime Fecha { get; set; }
}