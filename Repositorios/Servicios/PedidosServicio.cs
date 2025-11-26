using BD;
using Repositorios.Implementaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.Servicios
{
    public class PedidosServicio : IPedidosServicio
    {
        private readonly AppDbContext baseDeDatos;
        private readonly IPedidosServicio pedidosServicio;

        public PedidosServicio(AppDbContext baseDeDatos)
        {
            this.baseDeDatos = baseDeDatos;
        }

        //public Task<(bool, string)> ActualizarEstadoPedido()
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<(bool, string)> CrearNotaDePedido(CrearNotaDePedidoDTO crearNotaDePedidoDTO)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<(bool, string)> CrearRemito(CrearRemitoDTO crearRemitoDTO)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<(bool, List<DetalleRemitosDTO>)> ObtenerDetallesDeRemitos(int RemitoId)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<(bool, List<DetalleNotaDePedidoDTO>)> ObtenerDetallesNotaDePedido(int DetalleNotaDePedidoId)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
