using BD.Enums;

namespace DTO.DTOs_Remitos;

public class VerDetalleRemitoDTO
{
    public long Id { get; set; }
    public long RemitoId { get; set; }
    public long DetalleNotaDePedidoId  { get; set; }
    public string Recurso  { get; set; }
    public int Cantidad { get; set; }
    public int CantidadDespachada  { get; set; }
    public int? CantidadRecibida  { get; set; }
    public EnumEstadoRemito Estado { get; set; }
    public long? UsuarioQueRecibeId { get; set; }
    public string UsuarioQueRecibe { get; set; }
}