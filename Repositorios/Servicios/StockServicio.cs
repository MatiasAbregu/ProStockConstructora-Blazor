using BD;
using DTO.DTOs_Depositos;
using DTO.DTOs_Recursos;
using DTO.DTOs_Response;
using DTO.DTOs_Stock;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.Servicios
{
    public class StockServicio
    {
        private readonly AppDbContext baseDeDatos;
        public StockServicio(AppDbContext BaseDeDatos)
        {
            baseDeDatos = BaseDeDatos;
        }
        public async Task<Response<List<VerStockDTO>>> ObtenerStockPorObrasId(List<long> ObrasId)
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

                var Depositos = await baseDeDatos.Depositos.Where(d => ObrasId.Contains(d.ObraId)).Select(d => d.Id).ToListAsync();
                var Stocks = await baseDeDatos.Stocks.Where(s => Depositos.Contains(s.DepositoId)).ToListAsync();
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
    }
}
