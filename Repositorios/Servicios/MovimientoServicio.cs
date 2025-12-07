using BD;
using BD.Modelos;
using DTO.DTOs_Movimientos;
using DTO.DTOs_Response;
using DTO.Enum;
using Microsoft.EntityFrameworkCore;
using Repositorios.Implementaciones;

public class MovimientoServicio : IMovimientoServicio
{
    private readonly AppDbContext baseDeDatos;

    public MovimientoServicio(AppDbContext baseDeDatos)
    {
        this.baseDeDatos = baseDeDatos;
    }

    public async Task<Response<List<VerMovimientoDTO>>> ObtenerMovimientosPorEmpresa(long EmpresaId)
    {
        try
        {
            var Empresa = await baseDeDatos.Empresa.FirstOrDefaultAsync(e => e.Id == EmpresaId);
            if (Empresa == null)
                return new Response<List<VerMovimientoDTO>>()
                {
                    Estado = true,
                    Mensaje = "La empresa con ese ID no existe.",
                    Objeto = null
                };

            var movimientosDTO = await baseDeDatos.MovimientoStocks
                .Include(m => m.Stock).ThenInclude(s => s.Deposito).ThenInclude(d => d.Obra)
                .Include(m =>m.Stock).ThenInclude(s => s.Recurso)
                .Include(m => m.DetalleRemito).ThenInclude(dr => dr.Remito)
                .Where(m => m.Stock.Deposito.Obra.EmpresaId == Empresa.Id).Select(m => new VerMovimientoDTO()
                {
                    Id = m.Id,
                    RemitoId = m.DetalleRemito.Remito.Id,
                    NumeroRemito = m.DetalleRemito.Remito.NumeroRemito,
                    DepositoId = m.Stock.Deposito.Id,
                    Deposito = $"{m.Stock.Deposito.NombreDeposito} ({m.Stock.Deposito.CodigoDeposito})",
                    Recurso = $"{m.Stock.Recurso.Nombre} ({m.Stock.Recurso.CodigoISO})",
                    Cantidad = m.Cantidad,
                    TipoDeMovimiento = (TipoDeMovimiento)m.TipoDeMovimiento,
                    Fecha = m.Fecha,
                }).ToListAsync();

            if (movimientosDTO == null || movimientosDTO.Count == 0)
                return new Response<List<VerMovimientoDTO>>()
                {
                    Estado = true,
                    Mensaje = "¡No hay historial de movimientos entre depósitos en la empresa aún!",
                    Objeto = null
                };

            return new Response<List<VerMovimientoDTO>>()
            {
                Estado = true,
                Mensaje = null,
                Objeto = movimientosDTO
            };
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
            return new Response<List<VerMovimientoDTO>>()
            {
                Estado = false,
                Mensaje = "¡Hubo un error al cargar el historial de movimientos!",
                Objeto = null
            };
        }
    }

    public async Task MovimientoStockEntreDepositos(Deposito depositoDestino, MovimientoStock movimiento, BD.Enums.EnumEstadoRemito estado, int cantidadRecibida)
    {
        var stock = await baseDeDatos.Stocks.FirstOrDefaultAsync(s => s.Id == movimiento.StockId);

        if (depositoDestino == null) throw new Exception("Depósito destino no encontrado");
        if (stock == null) throw new Exception("Stock de origen no encontrado");
        if (cantidadRecibida < 0) throw new Exception("Cantidad recibida inválida");
        if (stock.Cantidad < cantidadRecibida)
            throw new Exception("Cantidad de stock insuficiente. Movimiento ilegal.");

        var stockDestinoExiste = await baseDeDatos.Stocks.FirstOrDefaultAsync(s => s.DepositoId == depositoDestino.Id && s.RecursoId == stock.RecursoId);

        if (estado == BD.Enums.EnumEstadoRemito.Recibido)
        {
            if (stockDestinoExiste == null)
            {
                stock.Cantidad -= cantidadRecibida;
                stockDestinoExiste = new Stock()
                {
                    DepositoId = depositoDestino.Id,
                    RecursoId = stock.RecursoId,
                    Cantidad = cantidadRecibida
                };
                baseDeDatos.Stocks.Add(stockDestinoExiste);
            }
            else
            {
                stock.Cantidad -= cantidadRecibida;
                stockDestinoExiste.Cantidad += cantidadRecibida;
            }

            await baseDeDatos.SaveChangesAsync();

            baseDeDatos.MovimientoStocks.Add(new MovimientoStock()
            {
                DetalleRemitoId = movimiento.DetalleRemitoId,
                StockId = stockDestinoExiste.Id,
                TipoDeMovimiento = (BD.Enums.TipoDeMovimiento)TipoDeMovimiento.Ingreso,
                Cantidad = cantidadRecibida,
                Fecha = DateTime.Now,
            });

            await baseDeDatos.SaveChangesAsync();
        }
        else if (estado == BD.Enums.EnumEstadoRemito.ParcialmenteRecibido)
        {
            if (stockDestinoExiste == null)
            {
                stock.Cantidad -= movimiento.Cantidad;
                stockDestinoExiste = new Stock()
                {
                    DepositoId = depositoDestino.Id,
                    RecursoId = stock.RecursoId,
                    Cantidad = cantidadRecibida
                };
                baseDeDatos.Stocks.Add(stockDestinoExiste);
            }
            else
            {
                stock.Cantidad -= movimiento.Cantidad;
                stockDestinoExiste.Cantidad += cantidadRecibida;
            }

            await baseDeDatos.SaveChangesAsync();

            baseDeDatos.MovimientoStocks.Add(new MovimientoStock()
            {
                DetalleRemitoId = movimiento.DetalleRemitoId,
                StockId = movimiento.StockId,
                TipoDeMovimiento = (BD.Enums.TipoDeMovimiento)TipoDeMovimiento.Faltante,
                Cantidad = movimiento.Cantidad - cantidadRecibida,
                Fecha = DateTime.Now,
            });
            baseDeDatos.MovimientoStocks.Add(new MovimientoStock()
            {
                DetalleRemitoId = movimiento.DetalleRemitoId,
                StockId = stockDestinoExiste.Id,
                TipoDeMovimiento = (BD.Enums.TipoDeMovimiento)TipoDeMovimiento.Ingreso,
                Cantidad = cantidadRecibida,
                Fecha = DateTime.Now,
            });
            await baseDeDatos.SaveChangesAsync();
        }
    }
}