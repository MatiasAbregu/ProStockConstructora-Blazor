using BD;
using DTO.DTOs_Depositos;
using DTO.DTOs_Recursos;
using DTO.DTOs_Response;
using DTO.DTOs_Stock;
using Microsoft.EntityFrameworkCore;
using Repositorios.Implementaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.Servicios
{
    public class StockServicio : IStockServicio
    {
        private readonly AppDbContext baseDeDatos;
        public StockServicio(AppDbContext BaseDeDatos)
        {
            baseDeDatos = BaseDeDatos;
        }

        public async Task<Response<List<VerStockDTO>>> ObtenerStockPorObrasId(List<long> ObrasId, long DepositoOrigenId)
        {
            try
            {
                if (ObrasId.Count < 1)
                    return new Response<List<VerStockDTO>>
                    {
                        Estado = true,
                        Mensaje = "El usuario no posee obras",
                        Objeto = null,
                    };

                var Depositos = await baseDeDatos.Depositos
                    .Where(d => ObrasId.Contains(d.ObraId) && d.Id != DepositoOrigenId)
                    .Select(d => d.Id).ToListAsync();
                var Stocks = await baseDeDatos.Stocks.Include(s => s.Deposito)
                    .Include(s => s.Recurso).ThenInclude(r => r.TipoMaterial)
                    .Include(s => s.Recurso).ThenInclude(r => r.UnidadMedida)
                    .Where(s => Depositos.Contains(s.DepositoId)).ToListAsync();
                var Lista = Stocks.Select(s => new VerStockDTO
                {
                    DepositoDTO = new VerDepositoDTO()
                    {
                        CodigoDeposito = s.Deposito.CodigoDeposito,
                        Domicilio = s.Deposito.Domicilio,
                        Id = s.Deposito.Id,
                        NombreDeposito = s.Deposito.NombreDeposito,
                        TipoDeposito = s.Deposito.TipoDeposito.ToString(),
                    },
                    ListadeRecursos = Stocks.Where(st => st.DepositoId == s.DepositoId).Select(st => new RecursosVerDTO()
                    {
                        Id = st.Recurso.Id,
                        CodigoISO = st.Recurso.CodigoISO,
                        Nombre = st.Recurso.Nombre,
                        TipoMaterial = st.Recurso.TipoMaterial.Nombre,
                        UnidadMedida = st.Recurso.UnidadMedida.Simbolo,
                        Cantidad = st.Cantidad,
                    }).ToList()
                }).DistinctBy(l => l.DepositoDTO.Id).ToList();

                return new Response<List<VerStockDTO>>
                {
                    Estado = true,
                    Mensaje = "Stock obtenido correctamente",
                    Objeto = Lista,
                };
            }

            catch (Exception e)
            {

                Console.WriteLine("Error" + e.Message);
                return new Response<List<VerStockDTO>>
                {
                    Estado = false,
                    Mensaje = "Error al obtener el stock correspondiente",
                    Objeto = null
                };
            }
        }

        public async Task<Response<List<VerStockDTO>>> ObtenerStocksDeEmpresaPorIdAdministrador(long UsuarioId, long DepositoOrigenId)
        {
            try
            {
                var rolesUsuario = await baseDeDatos.RolesUsuarios.Include(u => u.Rol)
                                               .Include(u => u.Usuario).ThenInclude(u => u.Empresa)
                                               .Where(ru => ru.UsuarioId == UsuarioId).ToListAsync();

                if (rolesUsuario.Count < 1)
                {
                    return new Response<List<VerStockDTO>>
                    {
                        Estado = true,
                        Mensaje = "¡El usuario no existe!",
                        Objeto = null
                    };
                }

                var empresaId = rolesUsuario.First().Usuario.EmpresaId;

                if (rolesUsuario.Any(ru => ru.Rol.NombreRol == "ADMINISTRADOR"))
                {
                    var listaStock = await baseDeDatos.Stocks
                        .Include(st => st.Deposito).ThenInclude(d => d.Obra)
                        .Include(st => st.Recurso).ThenInclude(r => r.TipoMaterial)
                        .Include(st => st.Recurso).ThenInclude(r => r.UnidadMedida)
                        .Where(st => st.Deposito.Obra.EmpresaId == empresaId && st.DepositoId != DepositoOrigenId)
                        .ToListAsync();

                    var listaVerStockDTO = listaStock.GroupBy(s => s.Deposito)
                                            .Select(g => new VerStockDTO()
                                            {
                                                DepositoDTO = new VerDepositoDTO
                                                {
                                                    CodigoDeposito = g.Key.CodigoDeposito,
                                                    Domicilio = g.Key.Domicilio,
                                                    Id = g.Key.Id,
                                                    NombreDeposito = g.Key.NombreDeposito,
                                                    TipoDeposito = g.Key.TipoDeposito.ToString(),
                                                },
                                                ListadeRecursos = g.Select(st => new RecursosVerDTO
                                                {
                                                    Id = st.Recurso.Id,
                                                    CodigoISO = st.Recurso.CodigoISO,
                                                    Nombre = st.Recurso.Nombre,
                                                    TipoMaterial = st.Recurso.TipoMaterial.Nombre,
                                                    UnidadMedida = st.Recurso.UnidadMedida.Simbolo,
                                                    Cantidad = st.Cantidad,
                                                }).ToList()
                                            }).ToList();
                    
                    return new Response<List<VerStockDTO>>
                    {
                        Estado = true,
                        Mensaje = "¡Stocks cargados con éxito!",
                        Objeto = listaVerStockDTO
                    };
                }
                else return new Response<List<VerStockDTO>>
                {
                    Estado = true,
                    Mensaje = "¡El usuario no es administrador!",
                    Objeto = null
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return new Response<List<VerStockDTO>>
                {
                    Estado = false,
                    Mensaje = "¡Hubo un error al obtener los stocks de la empresa!",
                    Objeto = null
                };
            }
        }
    }
}
