using BD;
using BD.Enums;
using BD.Modelos;
using DTO.DTOs_MaterialesYmaquinarias;
using DTO.DTOs_Recursos;
using DTO.Enum;
using Repositorios.Implementaciones;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.Servicios
{
    public class RecursosServicio : IRecursosServicio
    {
        private readonly AppDbContext baseDeDatos;

        public RecursosServicio(AppDbContext baseDeDatos)
        {
            this.baseDeDatos = baseDeDatos;
        }

        public async Task<(bool, string)> RecursoCargar(RecursosCargarDTO materialYmaquinaDTO, int DepositoId)
        {
            try
            {
                string codigoISO = materialYmaquinaDTO.CodigoISO.ToUpper();

                // Verificar que el depósito exista
                bool depositoExiste = await baseDeDatos.Depositos.AnyAsync(d => d.Id == DepositoId);
                if (!depositoExiste)
                    return (false, "El depósito no existe.");

                if (materialYmaquinaDTO.Cantidad <= 0)
                    return (false, "La cantidad debe ser mayor a 0.");

                //  Verificar si el recurso ya existe
                var (existeRecurso, mensaje) = await VerificarRecursoPorCodigoISO(codigoISO);

                if (existeRecurso)
                {
                    // Buscar el recurso existente
                    var recursoExistente = await baseDeDatos.Recursos
                        .FirstOrDefaultAsync(r => r.CodigoISO == codigoISO);

                    if (recursoExistente == null)
                        return (false, "Error inesperado: no se pudo obtener el recurso existente.");

                    // Verificar si tiene stock en ese depósito
                    var stockExistente = await baseDeDatos.Stocks
                        .FirstOrDefaultAsync(s => s.DepositoId == DepositoId && s.MaterialesyMaquinasId == recursoExistente.Id);

                    if (stockExistente != null)
                    {
                        // Ya existe en ese depósito → sumamos cantidad
                        stockExistente.Cantidad += materialYmaquinaDTO.Cantidad;
                        stockExistente.FechaIngreso = DateTime.Now;

                        baseDeDatos.Stocks.Update(stockExistente);
                        await baseDeDatos.SaveChangesAsync();

                        return (true, $"Cantidad actualizada. Nueva cantidad total: {stockExistente.Cantidad}");
                    }
                    else
                    {
                        // Recurso existe, pero no tiene stock en este depósito → crear stock nuevo
                        var nuevoStock = new Stock
                        {
                            DepositoId = DepositoId,
                            MaterialesyMaquinasId = recursoExistente.Id,
                            Cantidad = materialYmaquinaDTO.Cantidad,
                            FechaIngreso = DateTime.Now
                        };

                        await baseDeDatos.Stocks.AddAsync(nuevoStock);
                        await baseDeDatos.SaveChangesAsync();

                        return (true, "Stock agregado al depósito para un recurso existente.");
                    }
                }

                // Si el tipo de material y la unidad de media no existe, crearlo desde cero
                TipoMaterial? tipoMaterial = null;
                UnidadMedida? unidadMedida = null;

                if (materialYmaquinaDTO.Tipo == EnumTipoMaterialoMaquina.Material &&
                    materialYmaquinaDTO.TipoMaterial.Id == 0)
                {
                    tipoMaterial = await baseDeDatos.TipoMateriales
                        .FirstOrDefaultAsync(tm => tm.Nombre == materialYmaquinaDTO.TipoMaterial.Nombre.ToUpper());

                    if (tipoMaterial == null)
                    {
                        tipoMaterial = new TipoMaterial
                        {
                            Nombre = materialYmaquinaDTO.TipoMaterial.Nombre.ToUpper()
                        };
                        baseDeDatos.TipoMateriales.Add(tipoMaterial);
                        await baseDeDatos.SaveChangesAsync();
                    }
                }

                if (materialYmaquinaDTO.Tipo == EnumTipoMaterialoMaquina.Material &&
                    materialYmaquinaDTO.UnidadDeMedida.Id == 0)
                {
                    unidadMedida = await baseDeDatos.UnidadMedidas
                        .FirstOrDefaultAsync(um => um.Nombre == materialYmaquinaDTO.UnidadDeMedida.Nombre.ToUpper());

                    if (unidadMedida == null)
                    {
                        unidadMedida = new UnidadMedida
                        {
                            Nombre = materialYmaquinaDTO.UnidadDeMedida.Nombre.ToUpper(),
                            Simbolo = materialYmaquinaDTO.UnidadDeMedida.Abreviacion
                        };
                        baseDeDatos.UnidadMedidas.Add(unidadMedida);
                        await baseDeDatos.SaveChangesAsync();
                    }
                }

                // Crear recurso nuevo
                var nuevoRecurso = new Recursos
                {
                    CodigoISO = codigoISO,
                    Nombre = materialYmaquinaDTO.Nombre,      
                    Descripcion = materialYmaquinaDTO.Descripcion,
                    TipoMaterialId = tipoMaterial.Id,
                    UnidadMedidaId = unidadMedida?.Id
                };

                await baseDeDatos.Recursos.AddAsync(nuevoRecurso);
                await baseDeDatos.SaveChangesAsync();

                // Crear stock inicial
                var nuevoStockRecurso = new Stock
                {
                    DepositoId = DepositoId,
                    MaterialesyMaquinasId = nuevoRecurso.Id,
                    Cantidad = materialYmaquinaDTO.Cantidad,
                    FechaIngreso = DateTime.Now
                };

                await baseDeDatos.Stocks.AddAsync(nuevoStockRecurso);
                await baseDeDatos.SaveChangesAsync();

                return (true, "Material o máquina cargado con éxito.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return (false, "Error al cargar el material o máquina.");
            }
        }

        public async Task<(bool, object)> VerificarRecursoPorCodigoISO(string CodigoISO)
        {
            try
            {
                var recurso = await baseDeDatos.Recursos
                    .Include(r => r.UnidadMedida).Include(r => r.TipoMaterial)
                    .FirstOrDefaultAsync(r => r.CodigoISO.ToUpper() == CodigoISO.ToUpper());
                if (recurso != null)
                {
                    return (true, new RecursoStockVerDTO()
                    {
                        StockId = 0,
                        IdMaterial = recurso.Id,
                        CodigoISO = recurso.CodigoISO,
                        Nombre = recurso.Nombre,
                        TipoMaterial = recurso.TipoMaterial != null ? recurso.TipoMaterial.Nombre : "N/A",
                        UnidadDeMedida = recurso.UnidadMedida != null ? recurso.UnidadMedida.Nombre : "",
                        Descripcion = recurso.Descripcion,
                        Cantidad = 0
                    });
                }
                else
                {
                    return (false, "El recurso no existe.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return (false, "Error al verificar el recurso por código ISO.");
            }
        }

        public async Task<(bool, string)> RecursosTransladarAdeposito(RecursosTransladarDepositoDTO materialYmaquinaTransladarDeposito)
        {
            try
            {
                bool depositoOrigenExiste = await baseDeDatos.Depositos.AnyAsync(d => d.Id == materialYmaquinaTransladarDeposito.DepositoOrigenId);
                if (!depositoOrigenExiste)
                    return (false, "El deposito origen no existe");
                bool depositoDestinoExiste = await baseDeDatos.Depositos.AnyAsync(d => d.Id == materialYmaquinaTransladarDeposito.DepositoDestinoId);
                if (!depositoDestinoExiste)
                    return (false, "El deposito destino no existe");
                bool materialoMaquinaExiste = await baseDeDatos.Recursos.AnyAsync(m => m.Id == materialYmaquinaTransladarDeposito.MaterialYmaquinaId);
                if (!materialoMaquinaExiste)
                    return (false, "El material o maquina no existe");
                bool cantidadMaterialoMaquina = materialYmaquinaTransladarDeposito.Cantidad > 0;
                if (!cantidadMaterialoMaquina)
                    return (false, "La cantidad debe ser mayor a 0");
                var stockOrigen = await baseDeDatos.Stocks
                    .FirstOrDefaultAsync(s => s.DepositoId == materialYmaquinaTransladarDeposito.DepositoOrigenId &&
                                              s.MaterialesyMaquinasId == materialYmaquinaTransladarDeposito.MaterialYmaquinaId);
                if (stockOrigen == null || stockOrigen.Cantidad < materialYmaquinaTransladarDeposito.Cantidad)
                    return (false, "No hay suficiente stock en el deposito origen");

                stockOrigen.Cantidad -= materialYmaquinaTransladarDeposito.Cantidad;

                var stockDestino = await baseDeDatos.Stocks.FirstOrDefaultAsync(s => s.DepositoId == materialYmaquinaTransladarDeposito.DepositoDestinoId &&
                                              s.MaterialesyMaquinasId == materialYmaquinaTransladarDeposito.MaterialYmaquinaId);
                if (stockDestino != null)
                {
                    stockDestino.Cantidad += materialYmaquinaTransladarDeposito.Cantidad;
                    await baseDeDatos.SaveChangesAsync();
                    return (true, "Material o Maquina trasladado entre depositos con exito");
                }
                else
                {
                    stockDestino = new Stock
                    {
                        DepositoId = materialYmaquinaTransladarDeposito.DepositoDestinoId,
                        MaterialesyMaquinasId = materialYmaquinaTransladarDeposito.MaterialYmaquinaId,
                        Cantidad = materialYmaquinaTransladarDeposito.Cantidad,
                        FechaIngreso = DateTime.Now
                    };
                    await baseDeDatos.Stocks.AddAsync(stockDestino);
                    await baseDeDatos.SaveChangesAsync();
                    return (true, "Material o Maquina trasladado entre depositos con exito");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException.Message}");
                return (false, "Error al trasladar el material o maquina entre depositos");
            }
        }

        public async Task<(bool, List<RecursosVerDepositoDTO>)> RecursosVerDepositoDTO(int depositoId)
        {
            try
            {
                var resultado = await baseDeDatos.Stocks.Where(s => s.DepositoId == depositoId)
                   .Include(s => s.Deposito)
                   .Include(s => s.MaterialesyMaquinas)
                         .ThenInclude(m => m.TipoMaterial)
                   .Include(s => s.MaterialesyMaquinas)
                          .ThenInclude(t => t.UnidadMedida)
                   .Select(s => new RecursosVerDepositoDTO
                   {
                       Id = s.Id,
                       CodigoISO = s.MaterialesyMaquinas.CodigoISO,
                       Nombre = s.MaterialesyMaquinas.Nombre,
                       TipoRecursoTipoMaterial = $"{(s.MaterialesyMaquinas.TipoMaterial != null ? s.MaterialesyMaquinas.TipoMaterial.Nombre : "N/A")}",
                       UnidadMedida = s.MaterialesyMaquinas.UnidadMedida.Nombre,
                       Cantidad = s.Cantidad
                   })
                   .ToListAsync();
                return (true, resultado);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException.Message}");
                return (false, null);
            }
        }

        public async Task<(bool, List<RecursosPagPrincipalDTO>)> RecursosVerDTO(int EmpresaId)
        {
            try
            {
                var existe = await baseDeDatos.Obras.Where(s => s.EmpresaId == EmpresaId).ToListAsync();
                if (existe == null || existe.Count == 0)
                    return (false, null);
                var resultado = await baseDeDatos.Stocks.Where(s => s.Deposito.Obra.EmpresaId == EmpresaId)
                 .Include(s => s.Deposito)
                        .ThenInclude(s => s.Obra)
                 .Include(s => s.MaterialesyMaquinas)
                       .ThenInclude(m => m.TipoMaterial)
                 .Include(s => s.MaterialesyMaquinas)
                        .ThenInclude(t => t.UnidadMedida).Select(s => new RecursosPagPrincipalDTO()
                        {
                            CodigoISO = s.MaterialesyMaquinas.CodigoISO,
                            Nombre = s.MaterialesyMaquinas.Nombre,
                            UnidadMedida = s.MaterialesyMaquinas.UnidadMedida.Simbolo
                        }
                ).ToListAsync();
                return (true, resultado);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException.Message}");
                return (false, null);
            }
        }

        public async Task<(bool, RecursoStockVerDTO)> ObtenerRecursoPorStockId(int stockId)
        {
            var recurso = baseDeDatos.Stocks
                .Include(s => s.MaterialesyMaquinas)
                    .ThenInclude(r => r.UnidadMedida)
                .Include(s => s.MaterialesyMaquinas)
                    .ThenInclude(r => r.TipoMaterial)
                .FirstOrDefault(s => s.Id == stockId);

            if (recurso != null)
            {
                return (true, new RecursoStockVerDTO()
                {
                    StockId = stockId,
                    IdMaterial = recurso.MaterialesyMaquinasId,
                    CodigoISO = recurso.MaterialesyMaquinas.CodigoISO,
                    Nombre = recurso.MaterialesyMaquinas.Nombre,
                    TipoMaterial =
                    recurso.MaterialesyMaquinas.TipoMaterial != null ? recurso.MaterialesyMaquinas.TipoMaterial.Nombre : "N/A",
                    UnidadDeMedida =
                    recurso.MaterialesyMaquinas.UnidadMedida != null ? recurso.MaterialesyMaquinas.UnidadMedida.Nombre : "",
                    Descripcion = recurso.MaterialesyMaquinas.Descripcion,
                    Cantidad = recurso.Cantidad
                });
            }
            else
            {
                return (false, null);
            }
        }

        //public async Task<(bool, string)> RecursosActualizarStock(RecursosActualizarDTO dto, int depositoId)
        //{
        //    if (string.IsNullOrWhiteSpace(dto.Nombre))
        //        return (false, "El nombre del recurso no puede estar vacío.");

        //    if (string.IsNullOrWhiteSpace(dto.CodigoISO))
        //        return (false, "El código ISO no puede estar vacío.");

        //    if (dto.Cantidad < 0)
        //        return (false, "La cantidad no puede ser negativa.");

        //    var stock = await baseDeDatos.Stocks
        //        .FirstOrDefaultAsync(s => s.DepositoId == depositoId &&
        //                                  s.MaterialesyMaquinasId == dto.RecursoId);

        //    if (stock == null)
        //        return (false, "El recurso no existe en ese depósito.");

        //    var recurso = await baseDeDatos.MaterialesyMaquinas
        //        .FirstOrDefaultAsync(r => r.Id == dto.RecursoId);

        //    if (recurso == null)
        //        return (false, "El recurso no existe.");

        //    recurso.Nombre = dto.Nombre;
        //    recurso.CodigoISO = dto.CodigoISO.ToUpper();
        //    recurso.Descripcion = dto.Descripcion;

        //    TipoMaterial? tipoMaterial = null;
        //    UnidadMedida? unidadMedida = null;
        //    if (recurso.Tipo == EnumTipoMaterialOMaquina.Material)
        //    {

        //        tipoMaterial = await baseDeDatos.TipoMateriales
        //        .FirstOrDefaultAsync(t => t.Nombre.ToLower() == dto.TipoMaterial.ToLower());

        //        if (tipoMaterial == null)
        //        {
        //            tipoMaterial = new TipoMaterial
        //            {
        //                Nombre = dto.TipoMaterial
        //            };
        //            baseDeDatos.TipoMateriales.Add(tipoMaterial);
        //            await baseDeDatos.SaveChangesAsync();
        //        }

        //        unidadMedida = await baseDeDatos.UnidadMedidas
        //            .FirstOrDefaultAsync(u => u.Nombre.ToLower() == dto.UnidadDeMedida.ToLower());

        //        if (unidadMedida == null)
        //        {
        //            unidadMedida = new UnidadMedida
        //            {
        //                Nombre = dto.UnidadDeMedida,
        //                Simbolo = dto.UnidadDeMedida
        //            };
        //            baseDeDatos.UnidadMedidas.Add(unidadMedida);
        //            await baseDeDatos.SaveChangesAsync();
        //        }

        //        recurso.UnidadMedida = unidadMedida;
        //        recurso.TipoMaterial = tipoMaterial;
        //    }
        //    else
        //    {
        //        recurso.TipoMaterial = null;
        //        recurso.TipoMaterialId = null;
        //        recurso.UnidadMedida = null;
        //        recurso.UnidadMedidaId = null;
        //    }
        //    stock.Cantidad = dto.Cantidad;

        //    baseDeDatos.MaterialesyMaquinas.Update(recurso);
        //    baseDeDatos.Stocks.Update(stock);
        //    await baseDeDatos.SaveChangesAsync();
        //    return (true, "Stock actualizado con éxito.");
        //}

        public async Task<(bool, string)> RecursoEliminarStock(int StockId)
        {
            if (StockId > 0)
            {
                var stock = await baseDeDatos.Stocks.FindAsync(StockId);
                if (stock == null)
                    return (false, "El stock no existe.");
                baseDeDatos.Stocks.Remove(stock);
                await baseDeDatos.SaveChangesAsync();
                return (true, "Stock eliminado con éxito.");
            }
            else
            {
                return (false, "ID de stock inválido.");
            }

        }
    }
}

