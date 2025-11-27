using BD;
using BD.Modelos;
using DTO.DTOs_NotaDePedido;
using Repositorios.Implementaciones;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DTO.DTOs_Response;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Repositorios.Servicios
{
    public class NotaDePedidoServicio : INotaDePedidoServicio
    {
        private readonly AppDbContext BasedeDatos;

        public NotaDePedidoServicio(AppDbContext BasedeDatos)
        {
            this.BasedeDatos = BasedeDatos;

        }


        private async Task<string> GenerarNumeroNotaPedidoAsync()
        {
            var ultimoNumero = await BasedeDatos.NotaDePedidos
                .OrderByDescending(np => np.Id)
                .Select(np => np.Id)
                .FirstOrDefaultAsync();
            string numeroNotaPedido = $"NP-";
            if (ultimoNumero != null && ultimoNumero != 0)
            {
                numeroNotaPedido += $"{ultimoNumero}";
            }
            else
            {
                numeroNotaPedido += "1";
            }
            return numeroNotaPedido;
        }

        public async Task<Response<string>> ObtenerNumeroNotadePedidoSiguiente()
        {
            try
            {
                return new Response<string>
                {
                    Estado = true,
                    Mensaje = null,
                    Objeto = await GenerarNumeroNotaPedidoAsync()
                };
            }
            catch (Exception e)
            {

                Console.WriteLine("Error" + e.Message);
                return new Response<string>
                {
                    Estado = false,
                    Mensaje = "Error al obtener el número de nota de pedido.",
                    Objeto = null
                };
            }
        }

        public async Task<Response<string>> CrearNotaDePedido(CrearNotaDePedidoDTO NotadePedidoDTO)
        {
            try
            {
                if (NotadePedidoDTO.ListaDePedido.Count > 0)
                    return new Response<string>
                    {
                        Estado = true,
                        Mensaje = "La nota de pedido no tiene renglones",
                        Objeto = null
                    };
                var NotadePedido = new NotaDePedido()
                {
                    DepositoOrigenId = NotadePedidoDTO.DepositoOrigenId,
                    FechaEmision = DateTime.Now,
                    NumeroNotaPedido = await GenerarNumeroNotaPedidoAsync(),
                    UsuarioId = NotadePedidoDTO.UsuarioId,
                };
                BasedeDatos.Add(NotadePedido);
                await BasedeDatos.SaveChangesAsync();
                List<DetalleNotaDePedido> Detalles= new();

                foreach(var DetalleDTO in NotadePedidoDTO.ListaDePedido)
                {
                    var Detalle = new DetalleNotaDePedido()
                    {
                        Cantidad = DetalleDTO.Cantidad,
                        DepositoDestinoId = DetalleDTO.DepositoDestinoId,
                        RecursoId = DetalleDTO.RecursoId,
                        NotaDePedidoId = NotadePedido.Id,


                    };
                    Detalles.Add(Detalle);
                }
                BasedeDatos.AddRange(Detalles); 
                await BasedeDatos.SaveChangesAsync();
                return new Response<string>
                {
                    Estado = true,
                    Mensaje = null,
                    Objeto = "La nota de pedido se ha creado correctamente."
                };
              
            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e.Message);
                return new Response<string>
                {
                    Estado = false,
                    Mensaje = "Error al crear la nota de pedido.",
                    Objeto = null
                };


            }
        }
    }
}
