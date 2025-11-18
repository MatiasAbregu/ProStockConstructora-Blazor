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
using DTO.DTOs_Response;

namespace Repositorios.Servicios
{
    public class RecursosServicio : IRecursosServicio
    {
        private readonly AppDbContext baseDeDatos;

        public RecursosServicio(AppDbContext baseDeDatos)
        {
            this.baseDeDatos = baseDeDatos;
        }

        public async Task<Response<string>> RecursoCargar(RecursosCargarDTO materialYmaquinaDTO, long DepositoId)
        {
            try
            {
                if (materialYmaquinaDTO == null)
                    return new Response<string> { Estado = false, Mensaje = "Datos de recurso vacíos.", Objeto = null };

                if (string.IsNullOrWhiteSpace(materialYmaquinaDTO.CodigoISO))
                    return new Response<string> { Estado = false, Mensaje = "Debe especificar el Código ISO.", Objeto = null };

                string codigoISO = materialYmaquinaDTO.CodigoISO.ToUpper();

                bool depositoExiste = await baseDeDatos.Depositos.AnyAsync(d => d.Id == DepositoId);
                if (!depositoExiste)
                    return new Response<string> { Estado = false, Mensaje = "El depósito especificado no existe.", Objeto = null };

                if (materialYmaquinaDTO.Cantidad <= 0)
                    return new Response<string> { Estado = false, Mensaje = "La cantidad debe ser mayor a cero.", Objeto = null };


                //VERIFICAR SI YA EXISTE EL RECURSO

                var recursoExistente = await baseDeDatos.Recursos
                    .FirstOrDefaultAsync(r => r.CodigoISO == codigoISO);

                if (recursoExistente != null)
                {
                    var stockExistente = await baseDeDatos.Stocks
                        .FirstOrDefaultAsync(s => s.DepositoId == DepositoId &&
                                                  s.MaterialesyMaquinasId == recursoExistente.Id);

                    if (stockExistente != null)
                    {
                        stockExistente.Cantidad += materialYmaquinaDTO.Cantidad;
                        stockExistente.FechaIngreso = DateTime.Now;

                        baseDeDatos.Stocks.Update(stockExistente);
                        await baseDeDatos.SaveChangesAsync();

                        return new Response<string>
                        {
                            Estado = true,
                            Mensaje = "Stock actualizado para recurso existente.",
                            Objeto = null
                        };
                    }
                    else
                    {
                        var nuevoStock = new Stock
                        {
                            DepositoId = DepositoId,
                            MaterialesyMaquinasId = recursoExistente.Id,
                            Cantidad = materialYmaquinaDTO.Cantidad,
                            FechaIngreso = DateTime.Now
                        };

                        await baseDeDatos.Stocks.AddAsync(nuevoStock);
                        await baseDeDatos.SaveChangesAsync();

                        return new Response<string>
                        {
                            Estado = true,
                            Mensaje = "Stock creado para recurso existente.",
                            Objeto = null
                        };
                    }
                }

                // VALIDACIONES SEGÚN TIPO
                TipoMaterial? tipoMaterial = null;
                UnidadMedida? unidadMedida = null;

                bool esMaterial = materialYmaquinaDTO.TipoMaterial != null || materialYmaquinaDTO.UnidadDeMedida != null;

                // SI ES MATERIA, VALIDAMOS
                if (esMaterial)
                {
                    // Validar TipoMaterial
                    if (materialYmaquinaDTO.TipoMaterial == null || materialYmaquinaDTO.TipoMaterial.Id <=0)
                    {
                        return new Response<string>
                        {
                            Estado = false,
                            Mensaje = "Debe especificar el ID del tipo de material.",
                            Objeto = null
                        };
                    }

                    var IdTipo = materialYmaquinaDTO.TipoMaterial.Id;

                    tipoMaterial = await baseDeDatos.TipoMateriales
                        .FirstOrDefaultAsync(tm => tm.Id == IdTipo);

                    if (tipoMaterial == null)
                    {
                        return new Response<string>
                        {
                            Estado = false,
                            Mensaje = $"El tipo de material con el ID '{IdTipo}' no existe. Debe cargarlo antes.",
                            Objeto = null
                        };
                    }

                    // Validar Unidad Medida
                    if (materialYmaquinaDTO.UnidadDeMedida == null ||
                        string.IsNullOrWhiteSpace(materialYmaquinaDTO.UnidadDeMedida.Simbolo))
                    {
                        return new Response<string>
                        {
                            Estado = false,
                            Mensaje = "Debe especificar la unidad de medida.",
                            Objeto = null
                        };
                    }

                    string simboloUM = materialYmaquinaDTO.UnidadDeMedida.Simbolo.Trim().ToUpper();

                    unidadMedida = await baseDeDatos.UnidadMedidas
                        .FirstOrDefaultAsync(um => um.Simbolo.Trim().ToUpper() == simboloUM);

                    if (unidadMedida == null)
                    {
                        return new Response<string>
                        {
                            Estado = false,
                            Mensaje = $"La unidad de medida '{simboloUM}' no existe. Cárguela antes.",
                            Objeto = null
                        };
                    }
                }

                // CREAR NUEVO RECURSO
                var nuevoRecurso = new Recursos
                {
                    CodigoISO = codigoISO,
                    Nombre = materialYmaquinaDTO.Nombre,
                    Descripcion = materialYmaquinaDTO.Descripcion,
                    TipoMaterialId = tipoMaterial?.Id,
                    UnidadMedidaId = unidadMedida?.Id
                };

                await baseDeDatos.Recursos.AddAsync(nuevoRecurso);
                await baseDeDatos.SaveChangesAsync();

                // CREAR STOCK
                var nuevoStockRecurso = new Stock
                {
                    DepositoId = DepositoId,
                    MaterialesyMaquinasId = nuevoRecurso.Id,
                    Cantidad = materialYmaquinaDTO.Cantidad,
                    FechaIngreso = DateTime.Now
                };

                await baseDeDatos.Stocks.AddAsync(nuevoStockRecurso);
                await baseDeDatos.SaveChangesAsync();

                return new Response<string>
                {
                    Estado = true,
                    Mensaje = "Recurso y stock creados correctamente.",
                    Objeto = null
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new Response<string>
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error inesperado.",
                    Objeto = null
                };
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

        public async Task<Response<List<RecursosPagPrincipalDTO>>> RecursosVerDTO(int EmpresaId)
        {
            try
            {
                var existe = await baseDeDatos.Obras.Where(s => s.EmpresaId == EmpresaId).ToListAsync();
                if (existe == null || existe.Count == 0)
                {
                    Response<List<RecursosPagPrincipalDTO>>
                    res = new Response<List<RecursosPagPrincipalDTO>>()
                    { Estado = true };
                    return res;
                }
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
                Response<List<RecursosPagPrincipalDTO>> res2 = new Response<List<RecursosPagPrincipalDTO>>() { Estado = true, Objeto = resultado };
                return res2;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException.Message}");
                Response<List<RecursosPagPrincipalDTO>> res3 = new Response<List<RecursosPagPrincipalDTO>>() { Estado = false };
                return res3;
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

        public async Task<(bool, string)> RecursosActualizar(RecursosActualizarDTO recursoActualizarDTO, long recursoId)
        {
            var recurso = await baseDeDatos.Recursos.FindAsync(recursoId);
            if (recurso == null)
                return (false, "El recurso no existe.");
            recurso.CodigoISO = recursoActualizarDTO.CodigoISO.ToUpper();
            recurso.Nombre = recursoActualizarDTO.Nombre;
            recurso.Descripcion = recursoActualizarDTO.Descripcion;
            await baseDeDatos.SaveChangesAsync();
            return (true, "Recurso actualizado con éxito.");
        }

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
