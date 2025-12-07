using DTO.Enum;

namespace DTO.DTOs_Remitos;

public class VerRemitoDetalladoDTO
{
    public long Id { get; set; }
    public string NumeroRemito{ get; set; }
    public string DepositoOrigen { get; set; }
    public string Usuario { get; set; }
    public DateTime FechaEmision { get; set; }
    public List<VerDetalleRemitoDTO> Detalles { get; set; }
}