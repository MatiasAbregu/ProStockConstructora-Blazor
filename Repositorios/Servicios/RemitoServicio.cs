using BD;
using BD.Modelos;
using DTO.DTOs_Remitos;
using DTO.DTOs_Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositorios.Implementaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.Servicios
{
    public class RemitoServicio : IRemitoServicio
    {
        private readonly AppDbContext baseDeDatos;

        public RemitoServicio(AppDbContext BaseDeDatos)
        {
            baseDeDatos = BaseDeDatos;
        }
        public async Task<Response<string>> ObtenerNumeroRemitoSiguiente()
        {
            try
            {
                return new Response<string>
                {
                    Estado = true,
                    Mensaje = null,
                    Objeto = await GenerarNumeroRemitoAsync()
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

        private async Task<string> GenerarNumeroRemitoAsync()
        {
            var ultimoNumero = await baseDeDatos.Remitos
                .OrderByDescending(r => r.Id)
                .Select(r => r.Id)
                .FirstOrDefaultAsync();
            string numeroRemito = $"R-";
            if (ultimoNumero != null && ultimoNumero != 0)
            {
                numeroRemito += $"{ultimoNumero + 1}";
            }
            else
            {
                numeroRemito += "1";
            }
            return numeroRemito;
        }



    }

}
