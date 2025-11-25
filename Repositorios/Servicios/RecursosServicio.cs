using BD;
using BD.Enums;
using BD.Modelos;
using DTO.DTOs_MaterialesYmaquinarias;
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
                       Id = s.Id,
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
            throw new NotImplementedException();
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

        //public async Task<Response<string>>RecursosActualizar(RecursosActualizarDTO recursoActualizarDTO, long recursoId)
        //{
        //    var recurso = await baseDeDatos.Recursos.FindAsync(recursoId);
        //    if (recurso == null)
        //        return new Response<string>
        //        {
        //            Estado = false,
        //            Mensaje = "El recurso no existe.",
        //            Objeto = null
        //        };      
        //    recurso.CodigoISO = recursoActualizarDTO.CodigoISO.ToUpper();
        //    recurso.Nombre = recursoActualizarDTO.Nombre;
        //    recurso.Descripcion = recursoActualizarDTO.Descripcion;
        //    await baseDeDatos.SaveChangesAsync();
        //    return new Response<string>
        //    {
        //        Estado = true,
        //        Mensaje = "Recurso actualizado con éxito.",
        //        Objeto = null
        //    };               
        //}

        //public async Task<Response<string>>RecursoEliminarStock(long StockId)
        //{
        //   var stock = await baseDeDatos.Stocks.FindAsync(StockId);
        //   if (stock == null)
        //     return new Response<string>
        //     {
        //          Estado = false,
        //          Mensaje = "El stock no existe.",
        //          Objeto = null
        //     };
        //   baseDeDatos.Stocks.Remove(stock);
        //   await baseDeDatos.SaveChangesAsync();
        //   return new Response<string>
        //   {
        //        Estado = true,
        //        Mensaje = "Stock eliminado con éxito.",
        //        Objeto = null
        //   };
        //}
    }
}
