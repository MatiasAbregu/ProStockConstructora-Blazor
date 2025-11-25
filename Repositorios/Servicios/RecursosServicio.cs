using BD;
using BD.Enums;
using BD.Modelos;
using DTO.DTOs_Recursos;
using DTO.DTOs_Response;
using DTO.DTOs_Usuarios;
using DTO.Enum;
using Microsoft.EntityFrameworkCore;
using Repositorios.Implementaciones;
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

        public async Task<Response<List<RecursosVerDTO>>> ObtenerRecursosDeposito(long depositoId)
        {
            try
            {
                bool depositoExiste = await baseDeDatos.Depositos.AnyAsync(d => d.Id == depositoId);
                if (!depositoExiste) return new Response<List<RecursosVerDTO>>()
                {
                    Estado = true,
                    Mensaje = "¡No existe un depósito con ese ID!",
                    Objeto = null
                };

                var resultado = await baseDeDatos.Stocks.Where(s => s.DepositoId == depositoId)
                   .Include(s => s.Deposito)
                   .Include(s => s.Recurso)
                         .ThenInclude(r => r.TipoMaterial)
                   .Include(s => s.Recurso)
                          .ThenInclude(r => r.UnidadMedida)
                   .Select(s => new RecursosVerDTO
                   {
                       Id = s.Recurso.Id,
                       CodigoISO = s.Recurso.CodigoISO,
                       Nombre = s.Recurso.Nombre,
                       TipoMaterial = s.Recurso.TipoMaterial.Nombre,
                       UnidadMedida = s.Recurso.UnidadMedida.Nombre,
                       Cantidad = s.Cantidad
                   }).ToListAsync();

                if (resultado.Count == 0)
                    return new Response<List<RecursosVerDTO>>()
                    {
                        Objeto = null,
                        Mensaje = "No hay recursos cargados aún en el depósito.",
                        Estado = true
                    };

                return new Response<List<RecursosVerDTO>>
                {
                    Estado = true,
                    Mensaje = "¡Recursos cargados con éxito!",
                    Objeto = resultado
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new Response<List<RecursosVerDTO>>
                {
                    Estado = false,
                    Mensaje = "¡Hubo un error al cargar los recursos del depósito!",
                    Objeto = null
                };
            }
        }

        public async Task<Response<List<RecursosVerDTO>>> ObtenerRecursosEmpresa(long EmpresaId)
        {
            try
            {
                var existe = await baseDeDatos.Empresa.AnyAsync(s => s.Id == EmpresaId);
                if (!existe)
                    return new Response<List<RecursosVerDTO>>()
                    {
                        Estado = true,
                        Mensaje = "¡No existen una empresa con ese ID!",
                        Objeto = null
                    };

                var resultado = await baseDeDatos.Recursos.Where(s => s.EmpresaId == EmpresaId)
                        .Include(r => r.UnidadMedida).Include(r => r.TipoMaterial)
                        .Select(s => new RecursosVerDTO()
                        {
                            Id = s.Id,
                            CodigoISO = s.CodigoISO,
                            Nombre = s.Nombre,
                            UnidadMedida = s.UnidadMedida.Simbolo,
                            TipoMaterial = s.TipoMaterial.Nombre,
                        }
                ).ToListAsync();

                if (resultado.Count == 0)
                    return new Response<List<RecursosVerDTO>>()
                    {
                        Objeto = null,
                        Mensaje = "No hay recursos registrados aún en la empresa.",
                        Estado = true
                    };

                return new Response<List<RecursosVerDTO>>()
                {
                    Estado = true,
                    Mensaje = "¡Recursos cargados con éxito!",
                    Objeto = resultado
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new Response<List<RecursosVerDTO>>()
                {
                    Estado = false,
                    Mensaje = "¡Hubo un error al cargar los recursos de la empresa!",
                    Objeto = null
                };
            }
        }

        public async Task<Response<RecursosActualizarDTO>> ObtenerRecursoPorIdYODeposito(long? DepositoId, long RecursoId)
        {
            try
            {
                if (DepositoId != null && DepositoId != 0)
                {
                    var existe = await baseDeDatos.Depositos.AnyAsync(d => d.Id == DepositoId);
                    if(!existe) return new Response<RecursosActualizarDTO>()
                    {
                        Estado = true,
                        Mensaje = "No existe un depósito con ese ID.",
                        Objeto = null
                    };

                    var recurso = await baseDeDatos.Stocks
                        .Include(s => s.Recurso)
                            .ThenInclude(r => r.TipoMaterial)
                        .Include(s => s.Recurso)
                            .ThenInclude(r => r.UnidadMedida)
                        .Where(s => s.DepositoId == DepositoId && s.RecursoId == RecursoId)
                        .Select(s => new RecursosActualizarDTO()
                        {
                            Id = s.Recurso.Id,
                            CodigoISO = s.Recurso.CodigoISO,
                            Nombre = s.Recurso.Nombre,
                            TipoMaterialId = s.Recurso.TipoMaterial.Id,
                            UnidadDeMedidaId = s.Recurso.UnidadMedida.Id,
                            StockId = s.Id,
                            Cantidad = s.Cantidad
                        })
                        .FirstOrDefaultAsync();

                    return new Response<RecursosActualizarDTO>
                    {
                        Estado = true,
                        Mensaje = "¡Recurso cargado con éxito!",
                        Objeto = recurso
                    };
                }
                else
                {
                    var recurso = await baseDeDatos.Recursos
                        .Include(r => r.TipoMaterial)
                        .Include(r => r.UnidadMedida)
                        .Select(r => new RecursosActualizarDTO()
                        {
                            Id = r.Id,
                            CodigoISO = r.CodigoISO,
                            Nombre = r.Nombre,
                            TipoMaterialId = r.TipoMaterial.Id,
                            UnidadDeMedidaId = r.UnidadMedida.Id
                        })
                        .FirstOrDefaultAsync(r => r.Id == RecursoId);

                    if (recurso == null) return new Response<RecursosActualizarDTO>
                    {
                        Estado = true,
                        Mensaje = "No existe un recurso con ese ID.",
                        Objeto = null
                    };

                    return new Response<RecursosActualizarDTO>
                    {
                        Estado = true,
                        Mensaje = "¡Recurso cargado con éxito!",
                        Objeto = recurso
                    };
                }
            }
            catch (Exception ex) 
            { 
                Console.WriteLine("Error: " + ex.Message);
                return new Response<RecursosActualizarDTO>()
                {
                    Estado = false,
                    Mensaje = "¡Hubo un error al intentar cargar el recurso por su ID!",
                    Objeto = null
                };
            }
        }

        public async Task<Response<string>> RecursoCrear(RecursosCrearDTO recursoDTO)
        {
            try
            {
                bool depositoExiste = await baseDeDatos.Depositos.AnyAsync(d => d.Id == recursoDTO.DepositoId);
                if (!depositoExiste)
                    return new Response<string>
                    {
                        Estado = true,
                        Mensaje = "El depósito no existe dónde se añadirá el material no existe.",
                        Objeto = null
                    };

                var um = await baseDeDatos.UnidadMedidas
                    .FirstOrDefaultAsync(u => u.Id == recursoDTO.UnidadDeMedidaId);

                var tm = await baseDeDatos.TipoMateriales
                    .FirstOrDefaultAsync(t => t.Id == recursoDTO.TipoMaterialId);

                if (um == null || tm == null)
                    return new Response<string>
                    {
                        Estado = true,
                        Mensaje = "La unidad de medida o el tipo de material no existen.",
                        Objeto = null
                    };

                var existeRecurso = await baseDeDatos.Recursos
                    .AnyAsync(r => r.CodigoISO.ToUpper() == recursoDTO.CodigoISO.ToUpper());
                if (existeRecurso)
                    return new Response<string>
                    {
                        Estado = true,
                        Mensaje = "Ya existe un recurso con ese código ISO.",
                        Objeto = null
                    };


                var nuevoRecurso = new Recursos()
                {
                    CodigoISO = recursoDTO.CodigoISO.ToUpper(),
                    Nombre = recursoDTO.Nombre,
                    UnidadMedidaId = recursoDTO.UnidadDeMedidaId,
                    TipoMaterialId = recursoDTO.TipoMaterialId,
                    EmpresaId = recursoDTO.EmpresaId
                };
                baseDeDatos.Recursos.Add(nuevoRecurso);

                await baseDeDatos.SaveChangesAsync();
                baseDeDatos.Stocks.Add(new Stock()
                {
                    DepositoId = recursoDTO.DepositoId,
                    RecursoId = nuevoRecurso.Id,
                    Cantidad = recursoDTO.Cantidad
                });
                await baseDeDatos.SaveChangesAsync();

                return new Response<string>
                {
                    Estado = true,
                    Mensaje = null,
                    Objeto = "¡Recurso añadido al depósito con éxito!"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new Response<string>
                {
                    Estado = false,
                    Mensaje = "¡Hubo un error al crear un recurso para un depósito!",
                    Objeto = null
                };
            }
        }

        public async Task<Response<string>> RecursoAnadirPorISO(RecursoPorISODTO recursoDTO)
        {
            try
            {
                var recurso = await baseDeDatos.Recursos
                    .FirstOrDefaultAsync(r => r.CodigoISO.ToUpper() == recursoDTO.CodigoISO.ToUpper()
                    && r.EmpresaId == recursoDTO.EmpresaId);

                if (recurso == null)
                    return new Response<string>
                    {
                        Estado = true,
                        Mensaje = "No existe un recurso con ese código ISO en la empresa.",
                        Objeto = null
                    };

                bool depositoExiste = await baseDeDatos.Depositos.AnyAsync(d => d.Id == recursoDTO.DepositoId);
                if (!depositoExiste)
                    return new Response<string>()
                    {
                        Estado = true,
                        Mensaje = "El depósito con ese ID no existe.",
                        Objeto = null
                    };

                baseDeDatos.Stocks.Add(new Stock()
                {
                    DepositoId = recursoDTO.DepositoId,
                    RecursoId = recurso.Id,
                    Cantidad = recursoDTO.Cantidad
                });
                await baseDeDatos.SaveChangesAsync();

                return new Response<string>()
                {
                    Estado = true,
                    Mensaje = null,
                    Objeto = "¡Recurso añadido por su código ISO al depósito con éxito!"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new Response<string>
                {
                    Estado = false,
                    Mensaje = "¡Hubo un error al cargar recursos desde su código ISO!",
                    Objeto = null
                };
            }
        }

        public async Task<Response<string>> RecursoActualizar(long? DepositoId, long RecursoId, RecursosActualizarDTO recursoDTO)
        {
            try
            {
                var recurso = await baseDeDatos.Recursos.FirstOrDefaultAsync(r => r.Id == RecursoId);
                if (recurso == null) return new Response<string>()
                {
                    Estado = true,
                    Mensaje = "No existe un recurso con ese ID.",
                    Objeto = null
                };

                var codigoISOExiste = await baseDeDatos.Recursos
                    .AnyAsync(r => r.CodigoISO.ToUpper() == recursoDTO.CodigoISO.ToUpper()
                    && r.Id != RecursoId);
                if (codigoISOExiste) return new Response<string>()
                {
                    Estado = true,
                    Mensaje = "Ya existe un recurso con ese código ISO.",
                    Objeto = null
                };

                var um = await baseDeDatos.UnidadMedidas
                    .FirstOrDefaultAsync(u => u.Id == recursoDTO.UnidadDeMedidaId);

                var tm = await baseDeDatos.TipoMateriales
                    .FirstOrDefaultAsync(t => t.Id == recursoDTO.TipoMaterialId);

                if (um == null || tm == null)
                    return new Response<string>
                    {
                        Estado = true,
                        Mensaje = "La unidad de medida o el tipo de material no existen.",
                        Objeto = null
                    };

                recurso.CodigoISO = recursoDTO.CodigoISO.ToUpper();
                recurso.Nombre = recursoDTO.Nombre;
                recurso.TipoMaterialId = tm.Id;
                recurso.UnidadMedidaId = um.Id;
                await baseDeDatos.SaveChangesAsync();

                if (DepositoId != null && DepositoId != 0)
                {
                    var existeDeposito = await baseDeDatos.Depositos.AnyAsync(d => d.Id == DepositoId);
                    if (!existeDeposito) return new Response<string>()
                    {
                        Estado = true,
                        Mensaje = "No existe un depósito con ese ID.",
                        Objeto = null
                    };

                    var stock = await baseDeDatos.Stocks
                        .FirstOrDefaultAsync(s => s.Id == recursoDTO.StockId);

                    if (stock == null) return new Response<string>()
                    {
                        Estado = true,
                        Mensaje = "No existe stock para ese recurso en el depósito indicado.",
                        Objeto = null
                    };

                    stock.Cantidad = recursoDTO.Cantidad ?? 1;
                    await baseDeDatos.SaveChangesAsync();

                    return new Response<string>()
                    {
                        Estado = true,
                        Mensaje = null,
                        Objeto = "¡Recurso y stock actualizados con éxito!"
                    };
                }
                else return new Response<string>()
                {
                    Estado = true,
                    Mensaje = null,
                    Objeto = "¡Recurso actualizado con éxito!"
                };
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Error: " + ex.Message);
                return new Response<string>()
                {
                    Estado = false,
                    Mensaje = "¡Hubo un error al intentar actualizar el recurso!",
                    Objeto = null
                };
            }
        }
    }
}
